using UnityEngine;

namespace CombatGraph
{
    [System.Serializable, ClassName("No-Damage Attack")]
    class NoDamageAttack : AttacksHolder
    {
        public NoDamageAttack()
        {
            base.type = AttackType.NoDamage;
        }

        public override AttackData Attack(StatsHolder dealer)
        {
            if (Time.time < Timer.NextAvalibleTime) return new AttackData(false, 0, AttackStatus.NoDamage, atkName);

            Timer.ResetTimer();
            return new AttackData(true, 0, AttackStatus.NoDamage, atkName);
        }
    }
}