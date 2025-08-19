using UnityEngine;

namespace CombatGraph
{
    public class FieldNameAttribute : PropertyAttribute
    {
        public string fieldName;
        public int orderInMenu  = 0;
        public FieldNameAttribute(string name, int _order = 0)
        {
            fieldName = name;
            orderInMenu = _order;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false)]
    public class ClassNameAttribute : PropertyAttribute
    {
        public string className { get; }

        public ClassNameAttribute(string name)
        {
            className = name;
        }
    }  
}