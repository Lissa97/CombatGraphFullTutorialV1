using UnityEngine;

namespace CombatGraph
{

    [System.Serializable, ClassName("Crit Damage Increase Buff")]
    public class CritDamageIncrease : BattleEffect
    {
        [SerializeField, FieldName("Event Type")]
        internal EventManager.EventType actionName;
        [SerializeField, FieldName("Additional Crit Damage (%)")]
        internal int critDamageIncrease;
        [SerializeField, FieldName("Max Additional Crit Damage (%)")]
        internal int cap = 1;

        int currentBoost = 0;

        internal CritDamageIncrease()
        {
            // Default constructor for serialization
        }
        /// <summary>
        /// Creates a new Crit Damage Increase buff.
        /// </summary>
        /// <param name="eventType">Event type that triggers an buff action.</param>
        /// <param name="additionalCritDamage">Additional value that is added to critDamage when the buff is triggered.</param>
        /// <param name="cap">Once the sum of additional values exceeds the cap, buffs will no longer be triggered.</param>
        /// <param name="effectChance">Persentage of effect chance.</param>
        public CritDamageIncrease(
            EventManager.EventType eventType, 
            int additionalCritDamage, 
            int cap,
            int effectChance = 100)
        {
            this.actionName = eventType;
            this.critDamageIncrease = additionalCritDamage;
            this.cap = cap;
            this.effectChance = effectChance;
        }

        internal override void OnStart(StatsHolder _statsHolder, EventManager _actions)
        {
           base.OnStart(_statsHolder, _actions);
            actions.AddListener(actionName, IncreaseCreatDamage);
        }

        internal override string EffectName
        {
            get
            {
                return "Crit damage increased on " + critDamageIncrease + "% " + actionName;
            }
        }

        internal void IncreaseCreatDamage()
        {
            if (currentBoost >= cap) return;
            if (Random.Range(0, 100) >= effectChance) return;

            var additional = System.Math.Min(critDamageIncrease + currentBoost, cap) - currentBoost;
            if (additional <= 0) return;

            currentBoost += additional;
            statsHolder.CritDMG += 1f * additional / 100;
        }

        internal override void Reload()
        {
            currentBoost = 0;
        }

        internal override void OnDestroy()
        {
            actions.RemoveListener(actionName, IncreaseCreatDamage);
            statsHolder.CritDMG -= 1f * currentBoost / 100;
        }
    }
}