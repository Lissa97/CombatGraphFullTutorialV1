using UnityEngine;

namespace CombatGraph
{
    [System.Serializable, ClassName("Defense Increase Buff")]
    public class DefenseIncrease : BattleEffect
    {
        [SerializeField, FieldName("Event Type")]
        internal EventManager.EventType actionName;

        [SerializeField, FieldName("Addition Type")]
        internal AdditionType additionType;

        [SerializeField, FieldName("Additional Defense")]
        internal int additionDefense;
        [SerializeField, FieldName("Max Additional Defense")]
        internal int cap = 1;

        int currentBoost = 0;

        internal DefenseIncrease()
        {
            // Default constructor for serialization
        }
        /// <summary>
        /// Creates a new Max HP Increase buff.
        /// </summary>
        /// <param name="eventType">Event type that triggers an buff action.</param>
        /// <param name="additionType">Way of additional value calculating.</param>
        /// <param name="additionalDefense">Additional value that is added to Defense when the buff is triggered.</param>
        /// <param name="cap">Once the sum of additional values exceeds the cap, buffs will no longer be triggered.</param>
        /// <param name="effectChance">Persentage of effect chance.</param>
        public DefenseIncrease(
            EventManager.EventType eventType,
            AdditionType additionType,
            int additionalDefense,
            int cap,
            int effectChance = 100)
        {
            this.actionName = eventType;
            this.additionType = additionType;
            this.additionDefense = additionalDefense;
            this.cap = cap;
            this.effectChance = effectChance;
        }

        internal override void OnStart(StatsHolder _statsHolder, EventManager _actions)
        {
            base.OnStart(_statsHolder, _actions);
            actions.AddListener(actionName, Increase);
        }

        internal override string EffectName
        {
            get
            {
                if (additionType == AdditionType.Linear)
                {
                    return "Defense increased +" + additionDefense + " " + actionName;
                }
                return "Defense increased on " + additionDefense + "% " + actionName;
            }
        }

        internal void Increase()
        {
            if (currentBoost >= cap) return;
            if (Random.Range(0, 100) >= effectChance) return;

            var addition = Mathf.Min(additionDefense, cap - currentBoost);
            currentBoost += addition;

            if (additionType == AdditionType.Linear)
            {
                statsHolder.DefLinerBooster += addition;
            }
            else
            {
                statsHolder.DefPercentageBooster += addition;
            }
        }

        internal override void Reload()
        {
            currentBoost = 0;
        }

        internal override void OnDestroy()
        {
            actions.RemoveListener(actionName, Increase);

            if (additionType == AdditionType.Linear)
            {
                statsHolder.DefLinerBooster -= currentBoost;
            }
            else
            {
                statsHolder.DefPercentageBooster -= currentBoost;
            }
        }
    }
}