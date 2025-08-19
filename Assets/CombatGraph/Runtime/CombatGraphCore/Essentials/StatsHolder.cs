using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CombatGraph
{
    internal class StatsHolder
    {
        EventManager eventManager;
        HiddenActions hiddenActions;
        public StatsHolder(
            Stats _stats, 
            EventManager _actionsEvents, 
            HiddenActions _hiddenActions)
        {
            
            stats = _stats;
            eventManager = _actionsEvents;
            hiddenActions = _hiddenActions; 

            ReloadStats();
        }

        Stats stats;
        internal int MaxHp { get { return (int)((stats.MaxHp + hpLinerBuff) * hpPercentBuff); } }

        internal int Hp { get { return _hp; } }
        int _hp;
        float hpPercentBuff = 1;
        float hpLinerBuff = 0;

        public void Heal(int amount)
        {
            var totalHeal = Math.Min(amount, MaxHp - _hp);
            if (totalHeal <= 0) return; // no heal

            _hp += totalHeal;

            eventManager.Invoke(EventManager.EventType.OnHpChange, totalHeal);
            eventManager.Invoke<int>(EventManager.EventType.OnHeal, totalHeal);
        }

        /// <param name="damage"></param>
        /// <param name="pirsing"> damage ignoring blocks and shields</param>
        /// <returns></returns>
        public DamageData GetDamage(AttackData attackData, bool pirsing = false)
        {
            var result = DamageDataCalculate(attackData, pirsing);
            eventManager.Invoke(EventManager.EventType.OnTakeDamage, result);

            return result;
        }

        private DamageData DamageDataCalculate(AttackData attackData, bool pirsing)
        {
            //if (IsDead)
            if (IsDead)
            {
                return new DamageData(0, DamageStatus.Dead);
            }
            //if invincible
            if (invinsibleTime > Time.time)
            {
                return InvinsibleCalculation();
            }

            //if dodge
            if (UnityEngine.Random.Range(0, 1f) < DodgeRate)
            {
                return DodgeCalulation();
            }

            //if damage culculation without def and shield
            if (pirsing)
            {
                return PirsingDamageCalulation(attackData);
            }

            //if atk after applying def lesser than cap (some hp percent) ignore dmg
            if (
                damageCapPercent * ((stats.MaxHp + hpLinerBuff) * hpPercentBuff) + 0.01 >=
                attackData.DamageProduced * Mathf.Exp(-Defense / ((stats.MaxHp + hpLinerBuff) * hpPercentBuff))
            )
                return CapCalculation();

            //if atk after applying def lesser than cap (some constant) ignore dmg

            if (
               damageCapLiner + 0.01 >=
               attackData.DamageProduced * Mathf.Exp(-Defense / ((stats.MaxHp + hpLinerBuff) * hpPercentBuff))
            )
                return CapCalculation();

            return NormalCalculation(attackData);
        }

        private DamageData NormalCalculation(AttackData attackData)
        {
            var totalDamage = 0;

            if (_shield > 0)
            {
                _shield -= (int)(attackData.DamageProduced * Mathf.Exp((float)-Defense / ((stats.MaxHp + hpLinerBuff) * hpPercentBuff)));
                if (Shield < 0)
                {
                    totalDamage = -Shield;
                    _hp -= totalDamage;
                    _shield = 0;
                }
            }

            else
            {
                totalDamage = (int)(attackData.DamageProduced * Mathf.Exp(-Defense / ((stats.MaxHp + hpLinerBuff) * hpPercentBuff)));
                _hp -= totalDamage;
            }

            eventManager.Invoke(EventManager.EventType.OnHpChange, -totalDamage);


            if (_hp <= 0)
            {
                eventManager.Invoke(EventManager.EventType.OnDeath);
                LastDealer?.OnKill();
            }

            if(attackData.Status == AttackStatus.Critical)
            {
                return new DamageData(totalDamage, DamageStatus.Critical);
            }

            return new DamageData(totalDamage, DamageStatus.Landed);
        }

        private static DamageData CapCalculation()
        {
            return new DamageData(0, DamageStatus.Caped);
        }

        private DamageData PirsingDamageCalulation(AttackData attackData)
        {
            var damage = attackData.DamageProduced;

            if (damageCapPercent * ((stats.MaxHp + hpLinerBuff) * hpPercentBuff) + 0.01 >= damage)
                return new DamageData(0, DamageStatus.Caped);

            if (damageCapLiner + 0.01 >= damage)
                return new DamageData(0, DamageStatus.Caped);

            _hp -= damage;

            eventManager.Invoke(EventManager.EventType.OnHpChange, damage);

            if (_hp <= 0) eventManager.Invoke(EventManager.EventType.OnDeath);

            if(attackData.Status == AttackStatus.Critical)
                return new DamageData(damage, DamageStatus.Critical);
            return new DamageData(damage, DamageStatus.Landed);
        }

        private DamageData DodgeCalulation()
        {
            eventManager.Invoke(EventManager.EventType.OnDodge);
            return new DamageData(0, DamageStatus.Dodged);
        }

        private static DamageData InvinsibleCalculation()
        {
            return new DamageData(0, DamageStatus.Blocked);
        }

        public void AddHpBuff(AdditionType type, int buffValue)
        {
            if (type == AdditionType.Linear)
            {
                _hp += (int)(buffValue * hpPercentBuff);
                hpLinerBuff += buffValue;
                eventManager.Invoke(
                    EventManager.EventType.OnHpChange, 
                    (int)(buffValue * hpPercentBuff));
                return;
            }

            var buffValueFloat = 1.01f * buffValue / 100;
            _hp += (int)((stats.MaxHp + hpLinerBuff) * buffValueFloat);
            hpPercentBuff += buffValueFloat;
            eventManager.Invoke(
                EventManager.EventType.OnHpChange, 
                (int)((stats.MaxHp + hpLinerBuff) * buffValueFloat));
        }
        public int Mana { get; private set; }
        public int MaxMana { get { return stats.MaxMana; } }

        public void ChangeMana(int additionalMana)
        {
            if(additionalMana > 0) 
                eventManager.Invoke(EventManager.EventType.OnManaRestored, additionalMana);
            if(additionalMana < 0)
                eventManager.Invoke(EventManager.EventType.OnManaSpent, -additionalMana);

            Mana = Mathf.Min(Mana + additionalMana, MaxMana);
            eventManager.Invoke(EventManager.EventType.OnManaChange, additionalMana);
        }

        float _shield;
        public int Shield
        {
            get
            {
                return (int)_shield;
            }
        }

        internal int DefLinerBooster = 0;
        internal int DefPercentageBooster = 100;
        private int defense = 0;
        internal int Defense {
            get
            {
                return (int)((defense + DefLinerBooster) * (1f * DefPercentageBooster / 100));
            }
            set 
            {
                defense = value;
            } 
        }

        /// <summary>
        /// Ignore damage lesser than hp percent 
        /// </summary>
        float damageCapPercent = 0;
        float damageCapLiner = 0;

        List<float> percentCaps = new();
        List<float> linerCaps = new();
        public void SetCap(float newDamageCap, AdditionType buffType)
        {
            if(buffType == AdditionType.PercentageBased)
            {
                percentCaps.Add(newDamageCap);
                damageCapPercent = Math.Max(damageCapPercent, newDamageCap);
                return;
            }

            linerCaps.Add(newDamageCap);
            damageCapLiner = Math.Max(damageCapLiner, newDamageCap);
        }

        internal void RemoveCap(float newDamageCap, AdditionType buffType)
        {
            if(buffType == AdditionType.PercentageBased)
            {
                percentCaps.Remove(newDamageCap);
                damageCapPercent = percentCaps.Count > 0 ? percentCaps.Max() : 0;
                return;
            }

            linerCaps.Remove(newDamageCap);
            damageCapLiner = linerCaps.Count > 0 ? linerCaps.Max() : 0;
        }

        float invinsibleTime = 0;
        internal void AddInvincibilityTill(float time)
        {
            invinsibleTime = time;
        }

        /// <summary>
        /// General ATK + possible Crit. Invoke DealDamage event
        /// </summary>
        public AttackData DealDamage(int ATK, string atkName)
        {
            var result = CalculateAttackData(ATK, atkName);
            eventManager.Invoke(EventManager.EventType.OnAttack, result);
            return result;
        }

        private AttackData CalculateAttackData(int ATK, string atkName)
        {
            if (UnityEngine.Random.Range(0, 1f) < CritRate)
            {
                eventManager.Invoke(EventManager.EventType.OnCriticalHit);

                return new AttackData(true, ATK + (int)(ATK * CritDMG), AttackStatus.Critical, atkName);
            }

            return new AttackData(true, ATK, AttackStatus.Normal, atkName);
        }

        public float CritDMG { get; set; }
        public float CritRate { get; set; }
        public float DodgeRate { get; set; }
        public int ATK {
            get
            {
                return (int)((_Atk + AtkLinerBooster) * AtkPercentBooster);
            }
        }

        private int _Atk = 0;
       
        public bool IsDead {
            get
            {
                return _hp <= 0;
            }
        }

        public void ReloadStats()
        {
            _hp = stats.MaxHp;
            _Atk = stats.ATK;
            Defense = stats.Def;
            CritDMG = 1f * stats.CritDMG / 100;
            CritRate = 1f * stats.CritRate / 100;
            DodgeRate = 1f * stats.DodgeRate / 100;
            Mana = (int)(1f * stats.MaxMana * stats.maxOnStartPercent / 100);

            hpPercentBuff = 1;
            hpLinerBuff = 0;

            ReloadBoosters();

            eventManager.Invoke(EventManager.EventType.OnInitialize);
            eventManager.Invoke(EventManager.EventType.OnHpChange, Hp);
            eventManager.Invoke(EventManager.EventType.OnManaChange, Mana);
        }

        private void ReloadBoosters()
        {
            SpeedBooster = 1;
            AtkLinerBooster = 0;
            AtkPercentBooster = 1;
            invinsibleTime = 0;
        }

        public void AddShield(float shieldVal)
        {
            _shield += shieldVal;
        }

        internal StatsHolder LastWasAttacked;
        internal StatsHolder LastDealer;

        public void OnKill()
        {
            eventManager.Invoke(EventManager.EventType.OnKill);
        }

        public float SpeedBooster
        {
            get
            {
                return speedBooster;
            }
            set 
            { 
                speedBooster = value;
                hiddenActions.Invoke(HiddenActions.actionNames.onSpeedChange);
            }
        }

        private float speedBooster;
        public float AtkLinerBooster { get; set; }
        public float AtkPercentBooster { get; set; }
    }
}