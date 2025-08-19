using UnityEngine;

namespace CombatGraph
{
    [System.Serializable, ClassName("Invincibility Buff")]
    public class Invincibility : BattleEffect
    {
        [SerializeField, FieldName("Event Type")]
        internal EventManager.EventType actionName;

        [SerializeField, FieldName("Invincibility Duration (sec)")]
        internal float invincibilityTime = 0;
        internal override string EffectName
        { 
            get
            {
                return "Invincible on " + invincibilityTime + " sec " + actionName;
            }
        }

        internal Invincibility()
        {
            // Default constructor for serialization
        }
        /// <summary>
        /// Creates a new Invincibility buff.
        /// </summary>
        /// <param name="eventType">Event type that triggers an buff action.</param>
        /// <param name="invincibilityTime">Amout of HP that will be restored.</param>
        /// <param name="effectChance">Persentage of effect chance.</param>
        public Invincibility(
            EventManager.EventType eventType, 
            float invincibilityTime,
            int effectChance = 100)
        {
            this.actionName = eventType;
            this.invincibilityTime = invincibilityTime;
            this.effectChance = effectChance;
        }

        internal override void OnStart(StatsHolder _statsHolder, EventManager _actions)
        {
            base.OnStart(_statsHolder, _actions);
            actions.AddListener(actionName, Invinsiblity);
        }

        internal void Invinsiblity()
        {
            if (Random.Range(0, 100) >= effectChance) return;
            statsHolder.AddInvincibilityTill(Time.time + invincibilityTime);
        }
    }
}