using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatGraph
{

    class AttackCycle
    {
        public AttackCycle(
            StatsHolder _statsHolder,
            List<AttacksHolder> _attacks, 
            EventManager _actions, 
            HiddenActions _hiddenActions,
            MonoBehaviour _main)
        {
            
            statsHolder = _statsHolder; 
            mainEntity = _main;
            actions = _actions;
            hiddenActions = _hiddenActions;

            foreach (var att in _attacks)
            {
                attacks.Add(att.atkName, att);
                att.Timer.InitTimer();
            }

            hiddenActions.AddActionListener(HiddenActions.actionNames.onSpeedChange, UpdateSpeedBoosters);
        }

        MonoBehaviour mainEntity;
        EventManager actions;
        HiddenActions hiddenActions;
        StatsHolder statsHolder;
        Dictionary<string, AttacksHolder> attacks = new();
        PriorityQueue<AttacksHolder> attacksQueue = new();
        List<StatsHolder> statsHolders = new();
        AttackEssetials.AttackHandler onAttackAction;
        bool inAutoAttack = false;
        HashSet<string> silensedAttacks = new();

        public void StartAutoAttack(AttackEssetials.AttackHandler action)
        {
            onAttackAction = action;
            StartAutoAttack();
        }

        public void StartAutoAttack()
        {
            if (onAttackAction == null) return;
            if (inAutoAttack) return;
            if (attacks.Count == 0) return;

            foreach (var att in attacks.Values)
            {
                att.Timer.InitTimer();
            }

            inAutoAttack = true;
            Debug.Log("Starting auto attack for " + mainEntity.name);
            actions.AddListener(EventManager.EventType.OnManaRestored, TryPerformManaAttack);


            silensedAttacks.Clear();
            ReloadCooldowns();
            UpdateAttackQueue();
            mainEntity.StartCoroutine(AttackRun());
        }
        void TryPerformManaAttack()
        {
            if(manaAttacks.Count == 0) return;
            AttacksHolder performedAttack = null;

            foreach (var attack in manaAttacks)
            {
                if (attack.ManaRequire > statsHolder.Mana) continue;
                performedAttack = attack;
                break;
            }

            if (performedAttack != null)
            {
                manaAttacks.Remove(performedAttack);
                
                if(inAutoAttack)
                {
                    UpdateAttackQueue();
                    StartAutoAttack();
                }
            }
        }
        List<AttacksHolder> manaAttacks = new();
        int token = 0;
        float inavalibleTime = 0;
        IEnumerator AttackRun()
        {
            var currToken = token;
            
            if (attacksQueue.Count == 0) yield break;

            var currentAtk = attacksQueue.Dequeue();
            AttackData data;

            while (token == currToken)
            {
                inavalibleTime = Time.time >= inavalibleTime ? Time.time : inavalibleTime;

                yield return new WaitForSeconds(
                    System.Math.Max(
                        inavalibleTime - Time.time,
                        currentAtk.Timer.NextAvalibleTime - Time.time
                    ));

                if(token != currToken) yield break;

                if (currentAtk.type == AttacksHolder.AttackType.Mana && currentAtk.ManaRequire > statsHolder.Mana)
                {
                    manaAttacks.Add(currentAtk);

                    if (attacksQueue.Count == 0)
                    {
                        yield break;
                    }

                    currentAtk = attacksQueue.Dequeue();
                    continue;
                }

                inavalibleTime = Time.time + currentAtk.attackDuration + 0.1f;
                data = currentAtk.Attack(statsHolder);

                if (data.IsAvalible)
                    onAttackAction(data);
                else inavalibleTime = Time.time;

                attacksQueue.Enqueue(currentAtk, currentAtk.Timer.NextAvalibleTime);
                currentAtk = GetNextAttack();

                if(currentAtk == null) yield break;
            }
        }

        internal void ProhibitAttack(string attackName)
        {
            if(silensedAttacks.Contains(attackName)) return;
            silensedAttacks.Add(attackName);

            if(inAutoAttack)
            {
                UpdateAttackQueue();
                StartAttackCorutione();
            }
        }
        internal void AllowAttack(string attackName)
        {
            if (!silensedAttacks.Contains(attackName)) return;
            silensedAttacks.Remove(attackName);

            if (inAutoAttack)
            {
                UpdateAttackQueue();
                StartAttackCorutione();
            }
        }

        private AttacksHolder GetNextAttack()
        {
            AttacksHolder nextAttack = null;

            while(attacksQueue.Count > 0)
            {
                nextAttack = attacksQueue.Dequeue();

                if (!silensedAttacks.Contains(nextAttack.atkName))
                    break;
            }

            return nextAttack;
        }

        internal void ReloadAttackCycle()
        {
            mainEntity.StopCoroutine(AttackRun());
            UpdateToken();
            actions.RemoveListener(EventManager.EventType.OnManaChange, TryPerformManaAttack);
            inAutoAttack = false;
        }

        private void UpdateToken()
        {
            var newToken = Random.Range(0, 1000);
            if (token == newToken) newToken++;
            token = newToken;
        }

        internal AttackData Attack(string attackName)
        {
            if(inavalibleTime >= Time.time)
            {
                return new AttackData(false, 0, AttackStatus.Normal, attackName);
            }

            if(!attacks.ContainsKey(attackName))
            {
                Debug.LogError("Attack " + attackName + " not found");
                return new AttackData(false, 0, AttackStatus.Normal, attackName);
            }

            var result =  attacks[attackName].Attack(statsHolder);
            if (result.IsAvalible)
            {
                inavalibleTime = Time.time + attacks[attackName].attackDuration;
            }

            return result;
        }

        internal void UpdateAttackQueue()
        {
            manaAttacks.Clear();
            attacksQueue.Clear();

            foreach (var att in attacks.Values)
            {
                if(silensedAttacks.Contains(att.atkName)) continue;
                attacksQueue.Enqueue(att, att.Timer.NextAvalibleTime);
            }
        }
        internal void ReloadCooldowns()
        {
            foreach (var att in attacks.Values)
            {
                att.Timer.InitTimer();
            }
        }

        internal Timer GetTimer(string attackName)
        {
            if(!attacks.ContainsKey(attackName))
            {
                Debug.LogError("Attack " + attackName + " not found");
                return null;
            }
            return attacks[attackName].Timer;
        }

        internal bool IsPiercingAttack(string attackName)
        {
            return attacks[attackName].IsPiercing;
        }

        private void UpdateSpeedBoosters()
        {
            if (mainEntity == null || !mainEntity.isActiveAndEnabled) return;

            foreach (var att in attacks.Values)
            {
                att.Timer.ResetSpeedBooster(statsHolder.SpeedBooster);
            }

            UpdateAttackQueue();

            if(inAutoAttack && attacksQueue.Count > 0) StartAttackCorutione();
        }

        private void StartAttackCorutione()
        {
            UpdateToken();

            mainEntity.StartCoroutine(AttackRun());
        }
    }
}
