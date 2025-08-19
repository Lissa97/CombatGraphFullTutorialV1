using System;
using UnityEngine;

namespace CombatGraph
{
    [System.Serializable]
    abstract class AttacksHolder
    {
        abstract public AttackData Attack(StatsHolder dealer);
        [SerializeField, FieldName("Attack Name", -1)]
        internal string atkName;
        [SerializeField, FieldName("Delay After Attack (sec)")] internal float attackDelay = 0f;
        [SerializeField, FieldName("Attack Duration (sec)")] internal float attackDuration = 0f;

        internal virtual int ManaRequire { get; }

        internal AttackType type;
        internal virtual bool IsPiercing { get; }
        [SerializeField, FieldName("Ready On Start")]  
        internal bool readyOnStart = false;

        public enum AttackType
        {
            Mana,
            Basic,
            NoDamage
        }

        private Timer timer;
        internal Timer Timer { 
            get
            {
                if (timer == null)
                {
                    timer = new Timer(attackDelay, readyOnStart);
                }
                return timer;
            }
        }

        internal string Serialize()
        {
            return JsonUtility.ToJson(this);
        }

        internal static AttacksHolder Deserialize(string json, string type)
        {
            if(Type.GetType(type) == null || json == null)
            {
                //Debug.LogError($"Type {type} not found for deserialization of AttacksHolder.");
                return null;
            }
            return JsonUtility.FromJson(json, Type.GetType(type)) as AttacksHolder;
        }
    }
}