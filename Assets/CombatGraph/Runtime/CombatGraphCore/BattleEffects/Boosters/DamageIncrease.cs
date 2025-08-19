using UnityEngine;

namespace CombatGraph
{
    [System.Serializable, ClassName("Damage Increase Buff")]
    public class DamageIncrease : BattleEffect
    {
        [SerializeField, FieldName("Event Type")]
        internal EventManager.EventType actionName;
        [SerializeField, FieldName("Addition Type")]
        internal AdditionType buffType;
        [SerializeField, FieldName("Additional Damage")]
        internal int Atk;
        [SerializeField, FieldName("Max Additional Damage")]
        internal int cap = 1;

        [System.NonSerialized] int currentBoost = 0;

        internal DamageIncrease()
        {
            // Default constructor for serialization
        }
        /// <summary>
        /// Creates a new Damage Increase buff.
        /// </summary>
        /// <param name="eventType">Event type that triggers an buff action.</param>
        /// <param name="additionType">Way of additional value calculating.</param>
        /// <param name="additionalDamage">Additional value that is added to damage when the buff is triggered.</param>
        /// <param name="cap">Once the sum of additional values exceeds the cap, buffs will no longer be triggered.</param>
        /// <param name="effectChance">Persentage of effect chance.</param>
        public DamageIncrease(
            EventManager.EventType eventType, 
            AdditionType additionType, 
            int additionalDamage, 
            int cap, 
            int effectChance = 100)
        {
            this.actionName = eventType;
            this.buffType = additionType;
            Atk = additionalDamage;
            this.cap = cap;
            this.effectChance = effectChance;
        }

        internal override void OnStart(StatsHolder _statsHolder, EventManager _actions)
        {
            base.OnStart(_statsHolder, _actions);
            actions.AddListener(actionName, IncreaseAtk);
        }

        internal override string EffectName
        {
            get
            {
                if(buffType == AdditionType.Linear)
                {
                    return "Atk damage increased on +" + Atk + " " + actionName;

                }
                return "Atk damage increased on " + Atk + "% " + actionName;
            }
        }

        public void IncreaseAtk()
        {
            if (currentBoost >= cap) return;
            if (Random.Range(0, 100) >= effectChance) return;

            var addition = System.Math.Min(currentBoost + Atk, cap) - currentBoost;
            if(addition == 0) return;

            currentBoost = currentBoost + addition;

            if (buffType == AdditionType.Linear)
            {
                statsHolder.AtkLinerBooster += addition;
                return;
            }

            statsHolder.AtkPercentBooster += 1f * addition / 100;
        }

        internal override void Reload()
        {
            currentBoost = 0;
        }

        internal override void OnDestroy()
        {
            actions.RemoveListener(actionName, IncreaseAtk);

            if (buffType == AdditionType.Linear)
            {
                statsHolder.AtkLinerBooster -= currentBoost; 
            }

            statsHolder.AtkPercentBooster -= 1f * currentBoost / 100;
        }
    }
}