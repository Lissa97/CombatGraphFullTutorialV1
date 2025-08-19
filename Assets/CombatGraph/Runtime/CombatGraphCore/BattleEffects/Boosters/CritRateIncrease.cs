using UnityEngine;

namespace CombatGraph
{
    [System.Serializable, ClassName("Crit Rate Increase Buff")]
    public class CritRateIncrease : BattleEffect
    {
        [SerializeField, FieldName("Event Type")]
        internal EventManager.EventType actionName;

        [SerializeField, FieldName("Additional Crit Rate (%)")]
        internal int critRateIncrease;
        [SerializeField, FieldName("Max Additional Crit Rate (%)")]
        internal int cap = 1;

        int currentBoost = 0;

        internal CritRateIncrease()
        {
            // Default constructor for serialization
        }
        /// <summary>
        /// Creates a new Crit Rate Increase buff.
        /// </summary>
        /// <param name="eventType">Event type that triggers an buff action.</param>
        /// <param name="additionalCritRate">Additional value that is added to critRate when the buff is triggered.</param>
        /// <param name="cap">Once the sum of additional values exceeds the cap, buffs will no longer be triggered.</param>
        /// <param name="effectChance">Persentage of effect chance.</param>
        public CritRateIncrease(
            EventManager.EventType eventType, 
            int additionalCritRate, 
            int cap,
            int effectChance = 100)
        {
            this.actionName = eventType;
            this.critRateIncrease = additionalCritRate;
            this.cap = cap;
            this.effectChance = effectChance;
        }

        internal override void OnStart(StatsHolder _statsHolder, EventManager _actions)
        {
            base.OnStart(_statsHolder, _actions);
            actions.AddListener(actionName, IncreaseCritRate);
        }

        internal override string EffectName
        {
            get
            {
                return "Crit rate increased on " + critRateIncrease + "% " + actionName;
            }
        }

        internal void IncreaseCritRate()
        {
            if (currentBoost >= cap) return;
            if (Random.Range(0, 100) >= effectChance) return;

            var additional =  Mathf.Min(currentBoost + critRateIncrease, cap) - currentBoost;
            if (additional == 0) return; 

            currentBoost += additional;
            statsHolder.CritRate += 1f * additional / 100;
        }

        internal override void Reload()
        {
            currentBoost = 0;
        }

        internal override void OnDestroy()
        {
            actions.RemoveListener(actionName, IncreaseCritRate);
            statsHolder.CritRate -= 1f * currentBoost / 100;
        }
    }
}