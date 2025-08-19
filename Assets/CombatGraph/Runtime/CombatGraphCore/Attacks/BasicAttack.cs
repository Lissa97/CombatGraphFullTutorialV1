using UnityEngine;

namespace CombatGraph
{
    [System.Serializable, ClassName("Basic Attack")]
    class BasicAttack : AttacksHolder
    {
        public BasicAttack()
        {
            type = AttackType.Basic;
        }
        /// float attackDelay = 0;
        [SerializeField, FieldName("Attack Damage (% base dmg)")]
        protected int atkPercentDamage = 56;
        [SerializeField, FieldName("Ignore Defense And Shields")]
        internal bool isPiercing;
        internal override bool IsPiercing => isPiercing;
        public override AttackData Attack(StatsHolder dealer)
        {
            if (Time.time < Timer.NextAvalibleTime) 
                return new AttackData(false, 0, AttackStatus.Normal, atkName);

            Timer.ResetTimer();
            return dealer.DealDamage((int)((1f * atkPercentDamage) * dealer.ATK / 100f), atkName); 
        }
    }
}