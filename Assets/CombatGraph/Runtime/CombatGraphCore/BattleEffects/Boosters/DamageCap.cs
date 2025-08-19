using UnityEngine;

namespace CombatGraph
{
    [System.Serializable, ClassName("Set Damage Cap Buff")]
     public class DamageCap : BattleEffect
    {
        /// <summary>
        /// Percents of maxHp 0 - 100%
        /// </summary>
        [SerializeField, FieldName("Event Type")]
        internal EventManager.EventType actionName;
        [SerializeField, FieldName("Addition Type")]
        internal AdditionType actionType;
        [SerializeField, FieldName("Cap Value")]
        internal int capValue = 0;
        [SerializeField, FieldName("Max Cap Value")]
        internal int maxCap = 0;


        int currentCap;

        internal DamageCap()
        {
            // Default constructor for serialization
        }
        /// <summary>
        /// Creates a new Damage Cap buff.
        /// </summary>
        /// <param name="eventType">Event type that triggers an buff action.</param>
        /// <param name="additionType">Way of additional value calculating.</param>
        /// <param name="capValue">A value that represents the threshold below which an entity cannot take damage.</param>
        /// <param name="maxCapValue">Once the sum of cap values exceeds the cap, buffs will no longer be triggered.</param>
        /// <param name="effectChance">Persentage of effect chance.</param>
        public DamageCap(
            EventManager.EventType eventType, 
            AdditionType additionType, 
            int capValue, 
            int maxCapValue,
            int effectChance = 100)
        {
            this.actionName = eventType;
            this.actionType = additionType;
            this.capValue = capValue;
            this.maxCap = maxCapValue;
            this.effectChance = effectChance;
        }

        internal override void OnStart(StatsHolder _statsHolder, EventManager _actions)
        {
            base.OnStart(_statsHolder, _actions);
            actions.AddListener(actionName, AddCap);
        }

        internal override string EffectName
        {
            get
            {
                return "Cap set on " + capValue + "% from HP";
            }
        }

        public void AddCap()
        {
            if (currentCap >= maxCap) return;
            if (Random.Range(0, 100) >= effectChance) return;

            if(actionType == AdditionType.PercentageBased)
            {
                statsHolder.RemoveCap(1f * currentCap / 100, actionType);
                currentCap = System.Math.Min(capValue + currentCap, maxCap);
                statsHolder.SetCap(1f * currentCap / 100, actionType);
            }
            else
            {
                statsHolder.RemoveCap(currentCap, actionType);
                currentCap = System.Math.Min(capValue + currentCap, maxCap);
                statsHolder.SetCap(currentCap, actionType);
            }
        }

        internal override void OnDestroy()
        {
            statsHolder.RemoveCap(1f * currentCap / 100, actionType);
            actions.RemoveListener(actionName, AddCap);
        }
    }
}