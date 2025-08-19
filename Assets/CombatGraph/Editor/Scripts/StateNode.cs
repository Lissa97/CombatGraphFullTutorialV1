using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace CombatGraph.Editor
{
    public class StateNode : Node
    {
        public string effectName;
        private Dictionary<FieldNames, VisualElement> fields = new();
        private Dictionary<FieldNames, System.Action<object>> callbacks = new();

        public StateNode(string name)
        {
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets\\CombatGraph\\Editor\\Styles\\CombatSystemEditorStyle.uss"));
            title = "Stats";
            SetFixedPosition();
            InitializeFields();

            RefreshExpandedState();
            RefreshPorts();
        }

        private void SetFixedPosition()
        {
            style.flexDirection = FlexDirection.Column;
            style.alignSelf = Align.FlexStart;
            pickingMode = PickingMode.Ignore;
        }
        private void InitializeFields()
        {
            // Main Stats
            AddSection("Main Stats", new (FieldNames fieldName, VisualElement fieldType, string label)[]
            {
            //(FieldNames.Title, new TextField("Title"), "Title"),
            (FieldNames.Hp, new IntegerField("Max Hp"), "MaxHp"),
            (FieldNames.Def, new IntegerField("Defense"), "Defence"),
            (FieldNames.Atk, new IntegerField("Attack damage"), "Attack damage")
            });

            // Attack Details

            AddSection("Attack Details", new (FieldNames fieldName, VisualElement fieldType, string label)[]
            {
            (FieldNames.CritDamage, new IntegerField("Crit Damage (% of atk)"), "Crit Damage (% of atk)"),
            (FieldNames.CritRate, new IntegerField("Crit Rate (%)"), "Crit Rate (%)")
            });

            // Dodge
            AddSection("Dodge", new (FieldNames fieldName, VisualElement fieldType, string label)[]
            {
            (FieldNames.DodgeRate, new IntegerField("Dodge Rate (%)"), "Dodge Rate (%)")
            });

            // Mana
            AddSection("Mana", new (FieldNames fieldName, VisualElement fieldType, string label)[]
            {
            (FieldNames.MaxMana, new IntegerField("Max Mana"), "Max Mana"),
            (FieldNames.MaxManaStartPercent, new IntegerField("Mana on Start (%)"), "Max Mana on Start (%)")
            });
        }

        private void AddSection(string sectionName, (FieldNames fieldName, VisualElement element, string label)[] fieldInfos)
        {
            var header = new Label(sectionName);
            header.AddToClassList("big-bold-label");
            Add(header);

            foreach (var (fieldName, element, label) in fieldInfos)
            {
                if (element is TextField textField)
                    textField.RegisterValueChangedCallback(evt => TriggerCallBack(fieldName, evt.newValue));
                else if (element is IntegerField intField)
                    intField.RegisterValueChangedCallback(evt => TriggerCallBack(fieldName, evt.newValue));
                else if (element is FloatField floatField)
                    floatField.RegisterValueChangedCallback(evt => TriggerCallBack(fieldName, evt.newValue));

                // Optionally, you can add a Label element for the field
                var fieldLabel = new Label(label);
                fieldLabel.AddToClassList("little-bold-label");
                //mainContainer.Add(fieldLabel);

                fields[fieldName] = element;
                Add(element);
            }
        }
        private void TriggerCallBack(FieldNames fieldName, object value)
        {
            if (callbacks.ContainsKey(fieldName))
                callbacks[fieldName]?.Invoke(value);
        }

        public enum FieldNames
        {
            Title,
            Hp,
            Def,
            Atk,
            AtkSpeed,
            CritDamage,
            CritRate,
            DodgeRate,
            MaxMana,
            MaxManaStartPercent
        }

        internal void UpdateStatus(CombatGraph data)
        {
            if (data == null) return;

            SetFieldValue(FieldNames.Title, data.name);
            SetFieldValue(FieldNames.Hp, data.stats.MaxHp);
            SetFieldValue(FieldNames.Def, data.stats.Def);
            SetFieldValue(FieldNames.Atk, data.stats.ATK);
            SetFieldValue(FieldNames.CritDamage, data.stats.CritDMG);
            SetFieldValue(FieldNames.CritRate, data.stats.CritRate);
            SetFieldValue(FieldNames.DodgeRate, data.stats.DodgeRate);
            SetFieldValue(FieldNames.MaxMana, data.stats.MaxMana);
            SetFieldValue(FieldNames.MaxManaStartPercent, data.stats.maxOnStartPercent);
        }

        private void SetFieldValue(FieldNames fieldName, object value)
        {
            if (fields.TryGetValue(fieldName, out var element))
            {
                if (element is TextField textField)
                {
                    textField.SetValueWithoutNotify(value?.ToString());
                }
                else if (element is IntegerField intField && value is int intValue)
                {
                    intField.SetValueWithoutNotify(intValue);
                }
                else if (element is FloatField floatField && value is float floatValue)
                {
                    floatField.SetValueWithoutNotify(floatValue);
                }
            }
        }

        public void SetCallback(FieldNames fieldName, System.Action<object> callback)
        {
            callbacks[fieldName] = callback;
        }


    }
}
