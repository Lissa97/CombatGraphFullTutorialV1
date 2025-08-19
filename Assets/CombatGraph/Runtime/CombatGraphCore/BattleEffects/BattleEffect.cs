using System;
using System.Collections;
using UnityEngine;

namespace CombatGraph
{
    public enum AdditionType
    {
        PercentageBased,
        Linear
    }

    [System.Serializable]
    public abstract class BattleEffect 
    {

        [System.NonSerialized] internal StatsHolder statsHolder;
        [System.NonSerialized] internal EventManager actions;

        [SerializeField, FieldName("Effect Chance (%)")]
        internal int effectChance = 100; // percent 0 - 100%

        internal virtual bool isOverTime { get { return false; } }
        internal abstract string EffectName { get; }

        internal virtual void OnStart(StatsHolder _statsHolder, EventManager _actions)
        {
            statsHolder = _statsHolder;
            actions = _actions;
        }

        internal virtual IEnumerator OverTimeAction()
        {
            yield return null;
        }

        internal virtual void Reload()
        {

        }
        internal virtual void OnDestroy()
        {

        }

        internal string Serialize()
        {
            return JsonUtility.ToJson(this);
        }

        internal static BattleEffect Deserialize(string json, string type)
        {
            if(Type.GetType(type) is null) return null;
            return JsonUtility.FromJson(json, Type.GetType(type)) as BattleEffect;
        }
    }

}
