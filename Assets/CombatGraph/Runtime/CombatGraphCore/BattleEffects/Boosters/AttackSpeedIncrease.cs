using UnityEngine;

namespace CombatGraph
{
    [System.Serializable, ClassName("Attack Speed Increase Buff")]
    public class AttackSpeedIncrease : BattleEffect
    {
        [SerializeField, FieldName("Event Type")]
        internal EventManager.EventType actionName;

        /// <summary> 
        /// speed percents 0 - 100% add to basic 100%
        /// </summary>
        [SerializeField, FieldName("Attack Speed Multiplier (%)")]
        internal int atkspeedIncreased;
        [SerializeField, FieldName("Max Speed Boost Multiplier (%)")]
        internal int cap = 1;

        [System.NonSerialized] int currentBoost = 0;

        internal AttackSpeedIncrease()
        {
            // Default constructor for serialization
        }
        /// <summary>
        /// Creates a new Attack Speed Increase buff.
        /// </summary>
        /// <param name="eventType">Event type that triggers an buff action.</param>
        /// <param name="additionalSpeed">Additional value that is added to attackSpeed when the buff is triggered.</param>
        /// <param name="cap">Once the sum of additional values exceeds the cap, buffs will no longer be triggered.</param>
        /// <param name="effectChance">Persentage of effect chance.</param>
        public AttackSpeedIncrease(
            EventManager.EventType eventType, 
            int additionalSpeed, 
            int cap,
            int effectChance = 100)
        {
            this.actionName = eventType;
            this.atkspeedIncreased = additionalSpeed;
            this.cap = cap;
            this.effectChance = effectChance;
        }

        internal override void OnStart(StatsHolder _statsHolder, EventManager _actions)
        {
            base.OnStart(_statsHolder, _actions);
            actions.AddListener(actionName, IncreaseAtkSpeed);
        }

        internal override string EffectName
        {
            get
            {
                return "Atk speed increased on " + atkspeedIncreased + "% " + actionName;
            }
        }

        internal void IncreaseAtkSpeed()
        {
            if(currentBoost >= cap) return;
            if (Random.Range(0, 100) >= effectChance) return;

            var addition = System.Math.Min(atkspeedIncreased + currentBoost, cap) - currentBoost;
            if(addition <= 0) return;

            currentBoost += addition;
            statsHolder.SpeedBooster += 1f * addition / 100;
        }

        internal override void Reload()
        {
            currentBoost = 0;
        }

        internal override void OnDestroy()
        {
            actions.RemoveListener(actionName, IncreaseAtkSpeed);
            statsHolder.SpeedBooster -= 1f * currentBoost / 100;
        }
    }
}