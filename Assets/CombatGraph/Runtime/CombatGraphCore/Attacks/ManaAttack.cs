using UnityEngine;

namespace CombatGraph
{
    [System.Serializable, ClassName("Mana Attack")]
    class ManaAttack : AttacksHolder
    {
        public ManaAttack()
        {
            base.type = AttackType.Mana;
        }
        /// float attackDelay = 0;
        [SerializeField, FieldName("Attack Damage (% base dmg)")]
        protected int atkPercentDamage = 56;

        [SerializeField, FieldName("Ignore Defense And Shields")]
        internal bool isPiercing;
        internal override bool IsPiercing => isPiercing;

        [SerializeField, FieldName("Required Mana")]
        internal int manaRequire = 0;
        internal override int ManaRequire => manaRequire;
        public override AttackData Attack(StatsHolder dealer)
        {
            if (Time.time < Timer.NextAvalibleTime) return new AttackData(false, 0, AttackStatus.Normal, atkName);
            if(dealer.Mana < manaRequire) return new AttackData(false, 0, AttackStatus.Normal, atkName);
            
            dealer.ChangeMana(-manaRequire);
            Timer.ResetTimer();
            return dealer.DealDamage((int)((1f * atkPercentDamage) * dealer.ATK / 100f), atkName);
        }
    }
}