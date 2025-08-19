using UnityEngine;

namespace CombatGraph
{
    [System.Serializable, ClassName("Healing On Action Buff")]
    public class HealingOnAction : BattleEffect
    {
        [SerializeField, FieldName("Event Type")]
        internal EventManager.EventType actionName;

        [SerializeField, FieldName("Healed Hp Amount")] 
        internal int healingCount = 0;

        internal HealingOnAction()
        {
            // Default constructor for serialization
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventTYpe">Event type that triggers an buff action.</param>
        /// <param name="healedHpAmount">Amout of HP that will be restored.</param>
        /// <param name="effectChance">Persentage of effect chance.</param>
        public HealingOnAction(
            EventManager.EventType eventTYpe, 
            int healedHpAmount,
            int effectChance = 100)
        {
            this.actionName = eventTYpe;
            this.healingCount = healedHpAmount;
            this.effectChance = effectChance;
        }

        internal override void OnStart(StatsHolder _statsHolder, EventManager _actions) 
        {
            base.OnStart(_statsHolder, _actions);
            actions.AddListener(actionName, Heal);
        }

        internal override string EffectName
        {
            get
            {
                return "Healing " + healingCount + " on unknown event";
            }
        }

        internal void Heal()
        {
            if (Random.Range(0, 100) >= effectChance) return;
               
            statsHolder.Heal(healingCount);
        }

        internal override void OnDestroy()
        {
            actions.RemoveListener(actionName, Heal);
        }
    }

}