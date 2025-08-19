using UnityEngine;
using System.Collections.Generic;

namespace CombatGraph
{
    [System.Serializable]
    internal class CombatEntityDataSerializer
    {
        [SerializeField] 
        internal List<TypeData> buffs = new();

        [SerializeField]
        internal List<TypeData> attacks = new();

        [System.Serializable]
        internal struct TypeData
        {
            public string data;
            public string type;
            public Vector2 position;

            public TypeData(string _data, string _type, Vector2 _position)
            {
                data = _data;
                type = _type;
                position = _position;
            }
        }
    }
}



