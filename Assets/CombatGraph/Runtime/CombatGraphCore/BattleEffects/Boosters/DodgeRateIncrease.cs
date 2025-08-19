using UnityEngine;

namespace CombatGraph
{
    [System.Serializable, ClassName("Dodge Rate Increase Buff")]
    public class DodgeRateIncrease : BattleEffect
    {
        [SerializeField, FieldName("Event Type")]
        internal EventManager.EventType actionName;
        [SerializeField, FieldName("Additional Dodge Amount (%)")]
        internal int additionalDodge = 10;
        [SerializeField, FieldName("Max Additional Dodge Amount (%)")]
        internal int cap = 10;

        internal DodgeRateIncrease()
        {
            // Default constructor for serialization
        }
        /// <summary>
        /// Creates a new Dodge Rate Increase buff.
        /// </summary>
        /// <param name="eventType">Event type that triggers an buff action.</param>
        /// <param name="additionalDodgeRate">Additional value that is added to dodge when the buff is triggered.</param>
        /// <param name="cap">Once the sum of additional values exceeds the cap, buffs will no longer be triggered.</param>
        /// <param name="effectChance">Persentage of effect chance.</param>
        public DodgeRateIncrease(
            EventManager.EventType eventType,
            int additionalDodgeRate,
            int cap,
            int effectChance = 100)
        {
            this.actionName = eventType;
            this.additionalDodge = additionalDodgeRate;
            this.cap = cap;
            this.effectChance = effectChance;
        }

        internal override void OnStart(StatsHolder _statsHolder, EventManager _actions)
        {
            base.OnStart(_statsHolder, _actions);
            actions.AddListener(actionName, AddDodge);
        }

        internal override string EffectName
        {
            get
            {
                return "Add dodge " + additionalDodge + "% on" + actionName;
            }
        }

        private int currentDodge = 0;
        internal void AddDodge()
        {
            if (Random.Range(0, 100) >= effectChance) return;
            var additon = Mathf.Min(additionalDodge + currentDodge, cap) - currentDodge;

            currentDodge += additon;
            statsHolder.DodgeRate += 1f * additon / 100;
        }

        internal override void OnDestroy()
        {
            actions.RemoveListener(actionName, AddDodge);
            statsHolder.DodgeRate -= 1f * currentDodge / 100;
        }
    }
}