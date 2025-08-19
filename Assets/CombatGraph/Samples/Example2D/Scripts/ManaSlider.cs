using UnityEngine;
using UnityEngine.UI;
using CombatGraph;

namespace CombatGraphExample2D
{
    class ManaSlider : MonoBehaviour
    {
        [SerializeField] CombatEntity combatEntity;
        [SerializeField] Slider manaSlider;
        [SerializeField] Text manaText;

        private void Update()
        {
            manaSlider.value = combatEntity.CurrentMana;
            manaSlider.maxValue = combatEntity.MaxMana;

            manaText.text = $"{combatEntity.CurrentMana}/{combatEntity.MaxMana}";

        }
    }
}


