using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

namespace CombatGraph
{
    class HiddenActions
    {
        public enum actionNames
        {
            onSpeedChange
        };

        Dictionary<actionNames, UnityAction> actions = new();

        public void Invoke(actionNames actionName)
        {
            if (!actions.ContainsKey(actionName)) return;
            actions[actionName]?.Invoke();
        }

        public void AddActionListener(actionNames actionName, UnityAction action)
        {
            if (actions.ContainsKey(actionName))
            {
                actions[actionName] += action;
                return;
            }

            actions.Add(actionName, action);
        }

        public void RemoveActionListener(actionNames actionName, UnityAction action)
        {
            if (!actions.ContainsKey(actionName)) return;
            actions[actionName] -= action;
        }
    }
}
