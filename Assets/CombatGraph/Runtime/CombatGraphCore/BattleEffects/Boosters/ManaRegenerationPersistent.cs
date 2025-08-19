using System.Collections;
using UnityEngine;

namespace CombatGraph
{
    [System.Serializable, ClassName("Mana Regeneration Persistent Buff")]
    public class ManaRegenerationPersistent : BattleEffect
    {
        [SerializeField, FieldName("Recovery Rate (sec)")] 
        float manaRecoverSpeed = 0.1f;
        [SerializeField, FieldName("Recovered Mana")] 
        int manaRecoverCount = 0;
        override internal bool isOverTime { get { return true; } }

        internal ManaRegenerationPersistent()
        {
            // Default constructor for serialization
        }
        /// <summary>
        /// Creates a new Mana Regeneration buff that restores mana over time.
        /// </summary>
        /// <param name="manaRecoverySpeed">The duration of intervals at which Mana will be restored.</param>
        /// <param name="recoveredMana">Amout of Mana that will be restored.</param>
        /// <param name="effectChance">Persentage of effect chance.</param>
        public ManaRegenerationPersistent(
            float manaRecoverySpeed, 
            int recoveredMana,
            int effectChance = 100)
        {
            this.manaRecoverSpeed = manaRecoverySpeed;
            this.manaRecoverCount = recoveredMana;
            this.effectChance = effectChance;
        }

        internal override void OnStart(StatsHolder _statsHolder, EventManager _actions)
        {
            base.OnStart( _statsHolder, _actions );
        }

        internal override string EffectName
        {
            get
            {
                return "Passive Mana Regeneration " + manaRecoverCount + " over " + manaRecoverSpeed + " sec";
            }
        }

        internal override IEnumerator OverTimeAction()
        {
            while (true)
            {
                if (!statsHolder.IsDead && (Random.Range(0, 100) < effectChance))
                    statsHolder.ChangeMana(manaRecoverCount);

                yield return new WaitForSeconds(manaRecoverSpeed);
            }
        }
    }
}