using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using static CombatGraph.CombatEntityDataSerializer;
using System.Linq;

namespace CombatGraph
{
    [System.Serializable]
    internal class CombatGraph : ScriptableObject
    {
        private void OnEnable()
        {
            if (buffs.Count == serializer.buffs.Count && attacks.Count == serializer.attacks.Count)
            {
                return;
            }

#if UNITY_EDITOR
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CombatGraph/Editor/Icons/CombatEntityIco.png");
            EditorGUIUtility.SetIconForObject(this, icon);
#endif
            buffs.Clear();
            GetBuffsFromSerialization(buffs);

            attacks.Clear();
            GetAttacksFromSerialization(attacks);
        }

        [SerializeField] internal Stats stats = new();
        [SerializeField] private CombatEntityDataSerializer serializer = new();
        [System.NonSerialized] internal List<AttacksHolder> attacks = new();
        [System.NonSerialized] internal List<BattleEffect> buffs = new();
        [System.NonSerialized] internal Dictionary<object, Vector2> nodePosition = new();

        internal void PrepareBeforeSaving()
        {
            serializer.buffs.Clear();
            foreach (var buff in buffs)
            {
                if (!nodePosition.ContainsKey(buff)) continue;
                serializer.buffs.Add(
                    new TypeData(
                        buff.Serialize(),
                        buff.GetType().AssemblyQualifiedName,
                        nodePosition[buff]
                        ));
            }

            var set = new HashSet<string>();
            serializer.attacks.Clear();
            foreach (var attack in attacks)
            {
                while (attack.atkName == "" || set.Contains(attack.atkName))
                {
                    attack.atkName = attack.GetType().Name + Random.Range(1, 10000);
                }
                set.Add(attack.atkName);

                serializer.attacks.Add(
                    new TypeData(
                        attack.Serialize(),
                        attack.GetType().AssemblyQualifiedName,
                        nodePosition[attack]
                    ));
            }
        }

        private void GetBuffsFromSerialization(List<BattleEffect> output)
        {
            BattleEffect newEffect;

            foreach (var item in serializer.buffs)
            {
                newEffect = BattleEffect.Deserialize(item.data, item.type);
                if (newEffect == null) continue;
                output.Add(newEffect);
                nodePosition.TryAdd(newEffect, item.position);
            }
        }

        private void GetAttacksFromSerialization(List<AttacksHolder> output)
        {
            AttacksHolder attack;

            foreach (var item in serializer.attacks)
            {
                attack = AttacksHolder.Deserialize(item.data, item.type);
                if (attack == null) continue;

                output.Add(attack);
                nodePosition.TryAdd(attack, item.position);
            }
        }

        internal CombatGraph Copy()
        {
            CombatGraph copy = CreateInstance<CombatGraph>();
            copy.stats = stats.Copy();

            GetAttacksFromSerialization(copy.attacks);
            GetBuffsFromSerialization(copy.buffs);

            copy.nodePosition = nodePosition;
            return copy;
        }
    }
}


