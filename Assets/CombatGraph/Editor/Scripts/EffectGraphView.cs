using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEditor;
using System.Reflection;

namespace CombatGraph.Editor
{
    public class EffectGraphView : GraphView
    {
        public EffectGraphView()
        {
            RegisterSaveShortcut();
            //AddSaveButton(); // Add Save button at the top

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new ContentZoomer()
            {
                minScale = 0.1f,
                maxScale = 4.0f
            });
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            var contextMenuManipulator = new ContextualMenuManipulator(BuildContextMenu);
            this.AddManipulator(contextMenuManipulator);

            this.RegisterCallback<PointerDownEvent>(evt =>
            {
                if (evt.button == 1) // Right-click
                {
                    //lastMousePosition = evt.localPosition; // Save the correct mouse position
                    lastMousePosition = this.contentViewContainer.WorldToLocal(evt.originalMousePosition);
                }
            });

            // Здесь можно добавить фоновую сетку
            AddGridBackground();
            AddStateNode("State");
        }
        private Vector2 lastMousePosition;

        private void BuildContextMenu(ContextualMenuPopulateEvent evt)
        {

            if (selection.Any(x => x is StateNode))
            {
                evt.menu.MenuItems().RemoveAll(x => true);
                return;
            }
            ContextMenuEffect.BuildContextMenu(evt, this);
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();

            Insert(0, gridBackground);
        }

        private void AddSaveButton()
        {
            var saveButton = new Button(() => SaveData())
            {
                text = "Save"
            };
            saveButton.style.alignSelf = Align.FlexStart;
            saveButton.style.marginTop = 5;
            saveButton.style.marginLeft = 5;
            Add(saveButton);
        }

        List<EffectsNode<BattleEffect>> drawnBuffNodes = new();
        internal void AddBuffNode(Type type, Vector2 position, BattleEffect newBuff = null)
        {
            if (newBuff is null)
            {
                newBuff = Activator.CreateInstance(type,
        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
        null, null, null) as BattleEffect;
                combatEntity.buffs.Add(newBuff);

                position = lastMousePosition;
                combatEntity.nodePosition.Add(newBuff, position);
            }

            var effectNode = new EffectsNode<BattleEffect>(type, newBuff)
            {
                style = {
                left = position.x,
                top = position.y,
                width = 220,
                unityTextAlign = TextAnchor.LowerRight
            }
            };

            effectNode.onPositionChange += SetNewPosition;
            AddElement(effectNode);
            drawnBuffNodes.Add(effectNode);
        }

        List<EffectsNode<AttacksHolder>> drawnAttackNodes = new();
        internal void AddAttackNode(Type type, Vector2 position, AttacksHolder newAttack = null)
        {
            if (newAttack is null)
            {
                newAttack = Activator.CreateInstance(type) as AttacksHolder;
                combatEntity.attacks.Add(newAttack);

                position = lastMousePosition;
                combatEntity.nodePosition.Add(newAttack, position);
            }

            var effectNode = new EffectsNode<AttacksHolder>(type, newAttack)
            {
                style = {
                left = position.x,
                top = position.y,
                width = 220,
                unityTextAlign = TextAnchor.LowerRight
            }
            };

            effectNode.onPositionChange += SetNewPosition;
            AddElement(effectNode);
            drawnAttackNodes.Add(effectNode);
        }

        StateNode effectNode;
        StateNodeSaver StateNodeSaver;
        public void AddStateNode(string effectName)
        {
            var position = new Vector2(100, 200);
            effectNode = new StateNode(effectName);


            StateNodeSaver = new StateNodeSaver(effectNode);
            AddElement(effectNode);

        }
        CombatGraph combatEntity;
        internal void UpdateStatus(CombatGraph currentData)
        {
            SaveData();

            combatEntity = currentData;
            effectNode.UpdateStatus(currentData);
            StateNodeSaver.Update(currentData);

            foreach (var child in drawnBuffNodes)
            {
                RemoveElement(child);
            }
            drawnBuffNodes.Clear();

            foreach (var buff in currentData.buffs)
            {
                AddBuffNode(buff.GetType(), combatEntity.nodePosition[buff], buff);
            }

            foreach (var child in drawnAttackNodes)
            {
                RemoveElement(child);
            }

            drawnAttackNodes.Clear();

            foreach (var attack in currentData.attacks)
            {
                AddAttackNode(attack.GetType(), combatEntity.nodePosition[attack], attack);
            }

            GetTransform();
        }

        internal void SetNewPosition(object o, Vector2 position)
        {
            combatEntity.nodePosition[o] = position;
        }

        public void SaveData()
        {
            if (combatEntity == null) return;

            combatEntity.PrepareBeforeSaving();
            EditorUtility.SetDirty(combatEntity);
            AssetDatabase.SaveAssets();

            SaveTransform();
        }

        public void RegisterSaveShortcut()
        {
            this.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.ctrlKey && evt.keyCode == KeyCode.S)
                {
                    SaveData();
                }

                if (evt.keyCode == KeyCode.Delete)
                {
                    Delete();
                }
            });
        }

        public void Delete()
        {
            var queue = new Queue<ISelectable>(selection);

            if (queue.Count == 0) return;

            var child = queue.Peek();
            while (queue.Any())
            {
                child = queue.Dequeue();

                if (child is EffectsNode<BattleEffect> effectNode)
                {
                    drawnBuffNodes.Remove(effectNode);
                    combatEntity.nodePosition.Remove(effectNode.CurrentEffect);
                    combatEntity.buffs.Remove(effectNode.CurrentEffect);

                    RemoveElement(effectNode);
                }

                else if (child is EffectsNode<AttacksHolder> attackNode)
                {
                    drawnAttackNodes.Remove(attackNode);
                    combatEntity.nodePosition.Remove(attackNode.CurrentEffect);
                    combatEntity.attacks.Remove(attackNode.CurrentEffect);
                    RemoveElement(attackNode);
                }
            }
        }

        private void GetTransform()
        {
            Matrix4x4 transform = viewTransform.matrix;

            // You can serialize this to your asset or EditorPrefs, etc.
            Vector3 position = transform.GetColumn(3); // pan (x, y)
            Vector3 scale = new Vector3(transform.m00, transform.m11, transform.m22);

            Vector3 savedPan = SessionState.GetVector3(combatEntity.name + "_position", position);
            Vector3 savedZoom = SessionState.GetVector3(combatEntity.name + "_scale", scale);

            UpdateViewTransform(savedPan, savedZoom);
        }

        private void SaveTransform()
        {
            Matrix4x4 transform = viewTransform.matrix;

            // You can serialize this to your asset or EditorPrefs, etc.
            Vector3 position = transform.GetColumn(3); // pan (x, y)
            Vector3 scale = new Vector3(transform.m00, transform.m11, transform.m22);

            SessionState.SetVector3(combatEntity.name + "_position", position);
            SessionState.SetVector3(combatEntity.name + "_scale", scale);
        }

    }
}
