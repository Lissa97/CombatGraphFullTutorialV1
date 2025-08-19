using UnityEngine;

namespace CombatGraph
{
    [System.Serializable, ClassName("Shield Buff")]
    public class Shield : BattleEffect
    {
        [SerializeField, FieldName("Event Type")]
        internal EventManager.EventType actionName;

        [SerializeField, FieldName("Addition Type")]
        internal AdditionType shieldType;

        [SerializeField, FieldName("Additional Shield Amount")]
        internal int shieldAmount = 10;

        internal Shield()
        {
            // Default constructor for serialization
        }
        /// <summary>
        /// Creates a new Shield buff.
        /// </summary>
        /// <param name="eventType">Event type that triggers an buff action.</param>
        /// <param name="additionType">Way of additional value calculating.</param>
        /// <param name="shieldAmount">Additional value that is added to Shield when the buff is triggered.</param>
        /// <param name="effectChance">Persentage of effect chance.</param>
        public Shield(
            EventManager.EventType eventType, 
            AdditionType additionType, 
            int shieldAmount,
            int effectChance = 100)
        {
            this.actionName = eventType;
            this.shieldType = additionType;
            this.shieldAmount = shieldAmount;
            this.effectChance = effectChance;
        }

        internal override void OnStart(StatsHolder _statsHolder, EventManager _actions)
        {
            base.OnStart(_statsHolder, _actions);  
            actions.AddListener(actionName, AddShield);
        }

        internal override string EffectName
        {
            get
            {
                if(shieldType == AdditionType.PercentageBased)
                    return "Shield " + 1f * shieldAmount * statsHolder.MaxHp / 100 + " from maxHp " + actionName;

                return "Shield " + shieldAmount + " " + actionName;
            }
        }

        internal void AddShield()
        {
            if (Random.Range(0, 100) >= effectChance) return;
            if (shieldType == AdditionType.PercentageBased)
            {   
                statsHolder.AddShield((1f * shieldAmount * statsHolder.MaxHp / 100));
                return;
            }

            statsHolder.AddShield(shieldAmount);
        }

        internal override void OnDestroy()
        {
            actions.RemoveListener(actionName, AddShield);
        }
    }
}