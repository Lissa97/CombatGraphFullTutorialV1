using UnityEngine;

namespace CombatGraph
{
    [System.Serializable, ClassName("Max HP Increase Buff")]
    public class MaxHPIncrease : BattleEffect
    {
        [SerializeField, FieldName("Event Type")]
        internal EventManager.EventType actionName;

        [SerializeField, FieldName("Addition Type")]
        internal AdditionType buffType;

        [SerializeField, FieldName("Additional Hp")]
        internal int additionalHp;
        [SerializeField, FieldName("Max Additional Hp")]
        internal int cap = 1;

        int currentBoost = 0;

        internal MaxHPIncrease()
        {
            // Default constructor for serialization
        }
        /// <summary>
        /// Creates a new Max HP Increase buff.
        /// </summary>
        /// <param name="eventType">Event type that triggers an buff action.</param>
        /// <param name="additionType">Way of additional value calculating.</param>
        /// <param name="additionalHp">Additional value that is added to HP when the buff is triggered.</param>
        /// <param name="cap">Once the sum of additional values exceeds the cap, buffs will no longer be triggered.</param>
        /// <param name="effectChance">Persentage of effect chance.</param>
        public MaxHPIncrease(
            EventManager.EventType eventType, 
            AdditionType additionType, 
            int additionalHp, 
            int cap,
            int effectChance = 100)
        {
            this.actionName = eventType;
            this.buffType = additionType;
            this.additionalHp = additionalHp;
            this.cap = cap;
            this.effectChance = effectChance;
        }

        internal override void OnStart(StatsHolder _statsHolder, EventManager _actions)
        {
            base.OnStart(_statsHolder, _actions);
            actions.AddListener(actionName, IncreaseHp);
        }

        internal override string EffectName
        {
            get
            {
                if(buffType == AdditionType.Linear)
                {
                    return "Hp increased +" + additionalHp + " " + actionName;
                }
                return "Hp increased on " + additionalHp + "% " + actionName;
            }
        }

        internal void IncreaseHp()
        {
            if (currentBoost >= cap) return;
            if (Random.Range(0, 100) >= effectChance) return;

            statsHolder.AddHpBuff(buffType, -currentBoost);
            currentBoost = Mathf.Min(currentBoost + additionalHp, cap);
            statsHolder.AddHpBuff(buffType, currentBoost);
        }

        internal override void Reload()
        {
            currentBoost = 0;
        }

        internal override void OnDestroy()
        {
            actions.RemoveListener(actionName, IncreaseHp);
            statsHolder.AddHpBuff(buffType, -currentBoost);
        }
    }
}