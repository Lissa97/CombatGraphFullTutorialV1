using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using UnityEngine.UIElements;
using System;
using UnityEngine.Events;
using System.Linq;

namespace CombatGraph.Editor
{
    public class EffectsNode<T> : Node
    {
        public string effectName;
        public UnityAction<object, Vector2> onPositionChange;

        internal EffectsNode(Type type, T effectsBattle)
        {
            currentEffect = effectsBattle;
            styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets\\CombatGraph\\Editor\\Styles\\CombatSystemEditorStyle.uss"));

            effectName = type.GetCustomAttribute<ClassNameAttribute>()?.className;
            effectName = effectName ?? type.Name;

            extensionContainer.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);

            if (typeof(BattleEffect).IsAssignableFrom(type))
                titleContainer.style.backgroundColor = new Color(201f / 255, 184f / 255, 54f / 255);
            if (typeof(AttacksHolder).IsAssignableFrom(type))
                titleContainer.style.backgroundColor = new Color(240f / 255, 29f / 255, 67f / 255);//240, 29, 67

            Label titleLabel = this.Q<Label>("title-label");

            if (titleLabel != null)
            {
                titleLabel.style.color = new StyleColor(Color.black);
            }

            // Set up the node title
            title = effectName;

            var fieldName = "";

            IntegerField intField = null;
            FloatField floatField = null;
            EnumField enumField = null;
            Toggle boolField = null;
            TextField textField = null;

            // Iterate through fields and check for [SerializeField] attribute
            foreach (var field in
                type
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .OrderBy(x =>
                {
                    var attribute = x.GetCustomAttribute<FieldNameAttribute>();
                    return attribute == null ? 0 : attribute.orderInMenu;
                }))
            {
                if (!field.IsPublic && field.GetCustomAttribute<SerializeField>() == null) continue;

                fieldName = field.GetCustomAttribute<FieldNameAttribute>()?.fieldName;
                if (fieldName == null)
                    fieldName = field.Name;

                var lable = new Label(fieldName);
                lable.AddToClassList("little-bold-label");

                extensionContainer.Add(lable);

                if (field.FieldType == typeof(Int32))
                {
                    intField = new IntegerField { value = (int)field.GetValue(effectsBattle) };
                    intField.RegisterValueChangedCallback((evt) => field.SetValue(effectsBattle, evt.newValue));
                    extensionContainer.Add(intField);

                }

                else if (field.FieldType == typeof(float))
                {
                    floatField = new FloatField { value = (float)field.GetValue(effectsBattle) };
                    floatField.RegisterValueChangedCallback((evt) => field.SetValue(effectsBattle, evt.newValue));
                    extensionContainer.Add(floatField);
                }

                else if (field.FieldType.IsEnum)
                {
                    Type enumType = field.FieldType;
                    Enum currentValue = field.GetValue(effectsBattle) as Enum;

                    var enumNames = Enum.GetNames(enumType).OrderBy(name => name).ToList();

                    var popup = new PopupField<string>(enumNames, currentValue.ToString());

                    popup.RegisterValueChangedCallback(evt =>
                    {
                        var selectedName = evt.newValue;
                        var selectedEnum = (Enum)Enum.Parse(enumType, selectedName);
                        field.SetValue(effectsBattle, selectedEnum);
                    });

                    extensionContainer.Add(popup);
                }

                else if (field.FieldType == typeof(bool))
                {
                    boolField = new Toggle();
                    boolField.style.alignSelf = Align.FlexEnd;
                    boolField.value = (bool)field.GetValue(effectsBattle);
                    boolField.RegisterValueChangedCallback((evt) => field.SetValue(effectsBattle, evt.newValue));
                    extensionContainer.Add(boolField);
                }
                else if (field.FieldType == typeof(string))
                {
                    textField = new TextField();
                    textField.value = (string)field.GetValue(effectsBattle);
                    textField.RegisterValueChangedCallback((evt) => field.SetValue(effectsBattle, evt.newValue));
                    extensionContainer.Add(textField);
                }
            }

            RefreshExpandedState();
            RefreshPorts();
        }
        T currentEffect;
        internal T CurrentEffect
        {
            get { return currentEffect; }
        }
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            onPositionChange?.Invoke(currentEffect, newPos.position);
        }
    }
}
