using UnityEngine;
using System;

namespace CombatGraph
{
    [Serializable]
    struct Stats
    {
        [Header("MainStats")]
        public int MaxHp;
        public int Def;
        public int ATK;

        [Header("ATK details")]

        [Tooltip("Percent of ATK/100, 1 - 100% etc")]
        public int CritDMG; // additional Atk percent 0 - 100

        [Tooltip("Crit chance")]
        public int CritRate; // crit chance 0 - 100

        

        [Header("Dodge")]

        public int DodgeRate; // percent 0-100

        [Header("Mana")]
        [Tooltip("Persecond")]
        public int MaxMana;
        public int maxOnStartPercent;

        public Stats Copy()
        {
            var copy = new Stats();
            copy.MaxHp = MaxHp;
            copy.Def = Def;
            copy.ATK = ATK;
            copy.CritDMG = CritDMG;
            copy.CritRate = CritRate;
            copy.DodgeRate = DodgeRate;
            copy.MaxMana = MaxMana;
            copy.maxOnStartPercent = maxOnStartPercent;
            return copy;
        }

    }
}
