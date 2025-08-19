using UnityEngine;

namespace CombatGraph
{
    [System.Serializable, ClassName("No-Damage Mana Attack")]
    class NoDamageManaAttack : AttacksHolder
    {
        public NoDamageManaAttack()
        {
            type = AttackType.NoDamage;
        }
        [SerializeField, FieldName("Required Mana")]
        internal int manaRequire = 0;

        internal override int ManaRequire => manaRequire;
        public override AttackData Attack(StatsHolder dealer)
        {
            if (Time.time < Timer.NextAvalibleTime) return new AttackData(false, 0, AttackStatus.NoDamage, atkName);

            Timer.ResetTimer();
            return new AttackData(true, 0, AttackStatus.NoDamage, atkName);
        }
    }
}