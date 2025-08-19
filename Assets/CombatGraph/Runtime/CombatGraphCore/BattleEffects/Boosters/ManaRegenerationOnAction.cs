using UnityEngine;

namespace CombatGraph
{
    [System.Serializable, ClassName("Mana Regeneration On Action Buff")]
    public class ManaRegenerationOnAction : BattleEffect
    {
        [SerializeField, FieldName("Event Type")]
        internal EventManager.EventType actionName;
        [SerializeField, FieldName("Recovered Mana")]
        internal int additionalMana;

        internal ManaRegenerationOnAction()
        {
            // Default constructor for serialization
        }
        /// <summary>
        /// Creates a new Mana Regeneration buff.
        /// </summary>
        /// <param name="eventType">Event type that triggers an buff action.</param>
        /// <param name="recoveredMana">Amout of Mana that will be restored.</param>
        /// <param name="effectChance">Persentage of effect chance.</param>
        public ManaRegenerationOnAction(
            EventManager.EventType eventType, 
            int recoveredMana,
            int effectChance = 100)
        {
            this.actionName = eventType;
            this.additionalMana = recoveredMana;
            this.effectChance = effectChance;
        }

        internal override void OnStart(StatsHolder _statsHolder, EventManager _actions)
        {
            base.OnStart(_statsHolder, _actions);
            actions.AddListener(actionName, AddMana);
        }

        internal override string EffectName
        {
            get
            {
                return "Atk speed increased on " + additionalMana * 100 + "% " + actionName;
            }
        }

        internal void AddMana()
        {
            if (Random.Range(0, 100) >= effectChance) return;
            statsHolder.ChangeMana(additionalMana);
        }

        internal override void OnDestroy()
        {
            actions.RemoveListener(actionName, AddMana);
        }
    }
}