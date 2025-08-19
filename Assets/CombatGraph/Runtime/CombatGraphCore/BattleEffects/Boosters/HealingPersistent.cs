using System.Collections;
using UnityEngine;

namespace CombatGraph
{
    [System.Serializable, ClassName("Healing Persistent Buff")]
    public class HealingPersistent : BattleEffect
    {
        [SerializeField, FieldName("Healing Rate (sec)")] 
        float healingSpeed = 0.1f;
        [SerializeField, FieldName("Healed Hp Amount")] 
        int healnigCount = 0;
        override internal bool isOverTime { get { return true; } }

        internal HealingPersistent()
        {
            // Default constructor for serialization
        }
        /// <summary>
        /// Creates a new Healing Persistent buff that heals the target over time.
        /// </summary>
        /// <param name="healingSpeed">The duration of intervals at which HP will be restored.</param>
        /// <param name="healedHpAmount">Amout of HP that will be restored.</param>
        /// <param name="effectChance">Persentage of effect chance.</param>
        public HealingPersistent(
            float healingSpeed, 
            int healedHpAmount,
            int effectChance = 100)
        {
            this.healingSpeed = healingSpeed;
            this.healnigCount = healedHpAmount;
            this.effectChance = effectChance;
        }

        internal override void OnStart(StatsHolder _statsHolder, EventManager _actions)
        {
            base.OnStart(_statsHolder, _actions);
        }

        internal override string EffectName { get { 
                return "Passive Healing " + healnigCount + " over " + healingSpeed + " sec"; 
            }}

        internal override IEnumerator OverTimeAction()
        {
            while(true)
            {
                if(!statsHolder.IsDead && (Random.Range(0, 100) < effectChance)) 
                    statsHolder.Heal(healnigCount);

                yield return new WaitForSeconds(healingSpeed);
            }
        }
    }
}