using UnityEngine.UIElements;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace CombatGraph.Editor
{

    static class ContextMenuEffect
    {
        static public void BuildContextMenu(ContextualMenuPopulateEvent evt, EffectGraphView effectGraph)
        {
            AddBuffs(evt, effectGraph);
            AddAttacks(evt, effectGraph);

            evt.menu.MenuItems().RemoveAll(item =>
            {
                if (item is DropdownMenuAction action)
                {
                    return action.name == "Delete";
                }
                return false;
            });

            // 2. Add your own custom delete logic
            evt.menu.InsertAction(4, "Delete", action =>
            {
                effectGraph.Delete();
            });
        }

        private static void AddBuffs(ContextualMenuPopulateEvent evt, EffectGraphView effectGraph)
        {
            var folder = "Buffs";
            var buffName = "";

            Assembly assembly = typeof(BattleEffect).Assembly;
            var list = new List<(string, Type)>();

            // Find all types that inherit from EffectsBattle and are not abstract
            foreach (var type in assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(BattleEffect)) && !type.IsAbstract))
            {
                buffName = type.GetCustomAttribute<ClassNameAttribute>()?.className;
                if (buffName == null)
                    buffName = type.Name;
                list.Add((buffName, type));
            }

            foreach (var (name, type) in list.OrderBy(x => x.Item1))
            {
                evt.menu.AppendAction(folder + "/" + name, action => effectGraph.AddBuffNode(type, Vector2.zero), DropdownMenuAction.AlwaysEnabled);
            }

        }

        private static void AddAttacks(ContextualMenuPopulateEvent evt, EffectGraphView effectGraph)
        {
            var folder = "Attacks";
            var attackName = "";

            Assembly assembly = typeof(AttacksHolder).Assembly;
            var list = new List<(string, Type)>();

            // Find all types that inherit from EffectsBattle and are not abstract
            foreach (var type in assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(AttacksHolder)) && !type.IsAbstract))
            {
                attackName = type.GetCustomAttribute<ClassNameAttribute>()?.className;
                if (attackName == null)
                    attackName = type.Name;
                list.Add((attackName, type));
            }

            foreach (var (name, type) in list.OrderBy(x => x.Item1))
            {
                evt.menu.AppendAction(folder + "/" + name, action => effectGraph.AddAttackNode(type, Vector2.zero), DropdownMenuAction.AlwaysEnabled);
            }

        }
    }
}
