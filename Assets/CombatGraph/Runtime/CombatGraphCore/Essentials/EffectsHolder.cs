using System.Collections.Generic;
using UnityEngine;

namespace CombatGraph
{
    public class EffectsHolder
    {
        List<BattleEffect> effects;
        MonoBehaviour mainComponent;
        StatsHolder statsHolder;
        EventManager eventManager;
        internal EffectsHolder(
            List<BattleEffect> _effects,
            StatsHolder _statsHolder,
            EventManager _eventManager,
            MonoBehaviour _monoBehaviour)
        {
            effects = _effects;
            mainComponent = _monoBehaviour;
            statsHolder = _statsHolder;
            eventManager = _eventManager;


            foreach (var effect in effects)
            {
                effect.OnStart(statsHolder, eventManager);
                if (effect.isOverTime)
                    mainComponent.StartCoroutine(effect.OverTimeAction());
            }
        }

        internal void AddEffect(BattleEffect effect)
        {
            effects.Add(effect);

            effect.OnStart(statsHolder, eventManager);
            //Debug.Log("Added effect: " + effect.EffectName);    

            if (effect.isOverTime)
                mainComponent.StartCoroutine(effect.OverTimeAction());
        }

        internal void ReloadAll()
        {
            foreach (var effect in effects)
            {
                effect.Reload();
            }
        }

        internal void DestroyAll()
        {
            foreach (var effect in effects)
            {
                effect.OnDestroy();
                if (effect.isOverTime)
                    mainComponent.StopCoroutine(effect.OverTimeAction());
            }
        }
    }
}



