using UnityEditor;

namespace CombatGraph.Editor
{

    class StateNodeSaver
    {
        private CombatGraph combatEntity;
        public StateNodeSaver(StateNode stateNode)
        {
            stateNode.SetCallback(StateNode.FieldNames.Hp, (value) => ChangeInt(value, ref combatEntity.stats.MaxHp));
            stateNode.SetCallback(StateNode.FieldNames.Def, (value) => ChangeInt(value, ref combatEntity.stats.Def));
            stateNode.SetCallback(StateNode.FieldNames.Atk, (value) => ChangeInt(value, ref combatEntity.stats.ATK));
            stateNode.SetCallback(StateNode.FieldNames.CritDamage, (value) => ChangeInt(value, ref combatEntity.stats.CritDMG));
            stateNode.SetCallback(StateNode.FieldNames.CritRate, (value) => ChangeInt(value, ref combatEntity.stats.CritRate));
            stateNode.SetCallback(StateNode.FieldNames.DodgeRate, (value) => ChangeInt(value, ref combatEntity.stats.DodgeRate));
            stateNode.SetCallback(StateNode.FieldNames.MaxMana, (value) => ChangeInt(value, ref combatEntity.stats.MaxMana));
            stateNode.SetCallback(StateNode.FieldNames.MaxManaStartPercent, (value) => ChangeInt(value, ref combatEntity.stats.maxOnStartPercent));
        }
        public void Update(CombatGraph _combatEntityData)
        {
            combatEntity = _combatEntityData;
        }
        public void ChangeInt(object value, ref int prevVal)
        {
            if (value is int intValue)
            {
                prevVal = intValue;

                EditorUtility.SetDirty(combatEntity);
                AssetDatabase.SaveAssets();
            }
        }
        public void ChangeFloat(object value, ref float prevVal)
        {
            if (value is float floatValue)
            {
                prevVal = floatValue;

                EditorUtility.SetDirty(combatEntity);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
