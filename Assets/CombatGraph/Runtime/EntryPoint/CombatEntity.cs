using UnityEngine;
using System;


namespace CombatGraph
{ 
    public class CombatEntity : MonoBehaviour
    {
        [SerializeField] CombatGraph combatGraph;

        StatsHolder statsHolder;
        AttackCycle attackCycle;
        EffectsHolder effectsHolder;
        EventManager eventManager;

        private void Awake()
        {
            if (combatGraph == null)
            {
                Debug.LogError("Cannot find combatGraph in " + gameObject.name);
                return;
            }
            HiddenActions hiddenActions = new HiddenActions();

            var copyOfCombatGraph = combatGraph.Copy();

            statsHolder = new StatsHolder(copyOfCombatGraph.stats, EventManager, hiddenActions);
            attackCycle = new AttackCycle(statsHolder, copyOfCombatGraph.attacks, EventManager, hiddenActions, this);
            effectsHolder = new EffectsHolder(copyOfCombatGraph.buffs, statsHolder, EventManager, this);
        }
        /// <summary>
        /// Starts auto attacks, time depends.
        /// </summary>
        /// <param name="action">Method that will handel attacks using AttackData as parameter</param>
        public void StartAutoAttack(AttackEssetials.AttackHandler action)
        {
            attackCycle.StartAutoAttack(action);
        }
        public void StartAutoAttack()
        {
            attackCycle.StartAutoAttack();
        }
        /// <summary>
        /// Performs attack, returns produced damage.
        /// </summary>
        /// <param name="attackName"> Uniqueue attack name.</param>
        /// <returns>Includes produsedDamage, status etc.</returns>
        public AttackData Attack(string attackName)
        {
            return attackCycle.Attack(attackName);
        }
        /// <summary>
        /// Returns timer, that contains time till selected attack reset.
        /// </summary>
        /// <param name="attackName">Uniqueue attack name.</param>
        public Timer GetTimer(string attackName)
        {
            return attackCycle.GetTimer(attackName);
        }
        /// <summary>
        /// Takes away hp.
        /// </summary>
        /// <param name="attackData"> Description of an attack that can be obtained from StartAutoAttack, Attack, or constructed manually.</param>
        /// <param name="dealer"> Entity that deals damage.</param>
        /// <returns>Includes takenDamage, status etc.</returns>
        public DamageData TakeDamage(AttackData attackData, CombatEntity dealer)
        {
            statsHolder.LastDealer = dealer.StatsHolder;
            dealer.StatsHolder.LastWasAttacked = statsHolder;
            var result = statsHolder.GetDamage(attackData, dealer.IsPiercingAttack(attackData.AttackName));

            if(result.DamageTaken > 0) dealer.EventManager.Invoke(EventManager.EventType.OnDealDamage);
            dealer.EventManager.Invoke(EventManager.EventType.OnHitLanded, result);

            return result;
        }
        /// <summary>
        /// Prohibit attacks that should not be triggered by auto-attack.
        /// </summary>
        /// <param name="attackName"> Uniqueue attack name.</param>
        public void ProhibitAttack(string attackName)
        {
            attackCycle.ProhibitAttack(attackName);
        }
        /// <summary>
        /// Allow previously prohibited attacks to be triggered by auto-attack.
        /// </summary>
        /// <param name="attackName"> Uniqueue attack name.</param>
        public void AllowAttack(string attackName)
        {
            attackCycle.AllowAttack(attackName);
        }
        internal bool IsPiercingAttack(string attackName)
        {
            return attackCycle.IsPiercingAttack(attackName);
        }
        /// <summary>
        /// Dynamically adds new buffs that were not defined by default.
        /// </summary>

        public void AddNewBuff(BattleEffect effectsBattle)
        {
            effectsHolder.AddEffect(effectsBattle);
        }

        /// <summary>
        /// Sets default values of all stats and attacks.
        /// </summary>
        public void ReloadEntity()
        {
            statsHolder.ReloadStats();
            attackCycle.ReloadAttackCycle();
            effectsHolder.ReloadAll();
        }

        private void OnDestroy()
        {
            effectsHolder.DestroyAll();
        }
        /// <summary>
        /// Return current hp
        /// </summary>
        public int CurrentHp { get { return Math.Max(statsHolder.Hp, 0); } }
        /// <summary>
        /// Returns max hp after applying all boosters
        /// </summary>
        public int MaxHp { get { return statsHolder.MaxHp; } }
        /// <summary>
        /// Returns basic damage after applying all boosters
        /// </summary>
        public int AttackDamage { get { return statsHolder.ATK; } }
        /// <summary>
        /// Return current mana
        /// </summary>
        public int CurrentMana { get { return Math.Max(statsHolder.Mana, 0); } }
        /// <summary>
        /// Returns max mana after applying all boosters
        /// </summary>
        public int MaxMana { get { return statsHolder.MaxMana; } }
        /// <summary>
        /// Returns defence after applying all boosters
        /// </summary>
        public int Defense { get { return statsHolder.Defense; } }
        /// <summary>
        /// Returns current shield  value
        /// </summary>
        public int CurrentShield { get { return statsHolder.Shield; } }

        public EventManager EventManager {
            get 
            { 
                if(eventManager == null) eventManager = new EventManager();
                return eventManager; 
            }
        }

        public EffectsHolder EffectsHolder
        {
            get
            {
                return effectsHolder;
            }
        }

        internal StatsHolder StatsHolder { get { return statsHolder; } }
    }
}