using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace CombatGraph
{
    public class EventManager
    {
        public enum EventType
        {
            OnInitialize,
            OnHpChange,
            OnHeal,
            OnManaChange,
            OnManaRestored,
            OnAttack,
            OnDealDamage,
            OnTakeDamage,
            OnDodge,
            OnDeath,
            OnCriticalHit,
            OnKill,
            OnManaSpent,
            OnHitLanded
        };

        Dictionary<EventType, Delegate> actions = new();
        Dictionary<EventType, UnityAction> voidActions = new();

        internal void Invoke(EventType type)
        {
            if (voidActions.ContainsKey(type))
            {
                voidActions[type].Invoke();
            }
        }

        internal void Invoke<T>(EventType type, T args)
        {
            if (actions.TryGetValue(type, out var del))
            {
                (del as UnityAction<T>)?.Invoke(args);
            }

            Invoke(type);
        }

        public void AddListener(EventType type, UnityAction listener)
        {
            if(type == EventType.OnInitialize)
            {
                listener.Invoke();
                return;
            }

            if (voidActions.ContainsKey(type))
            {
                voidActions[type] += listener;
            }
            else
            {
                voidActions[type] = listener;
            }
        }

        public void AddListener<T>(EventType type, UnityAction<T> listener)
        {
            if (type == EventType.OnInitialize)
            {
                //action.Invoke();
                return;
            }

            if (actions.TryGetValue(type, out var existing))
            {
                actions[type] = Delegate.Combine(existing, listener);
            }
            else
            {
                actions[type] = listener;
            }
        }

        public void RemoveListener(EventType type, UnityAction listener)
        {
            if (!voidActions.ContainsKey(type)) return;

            voidActions.Remove(type);
        }

        public void RemoveListener<T>(EventType type, UnityAction<T> listener)
        {
            if (actions.TryGetValue(type, out var existing))
            {
                actions[type] = Delegate.Remove(existing, listener);
            }
        }

    }
}
