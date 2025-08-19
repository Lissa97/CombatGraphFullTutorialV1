using CombatGraph;
using UnityEngine;
using UnityEngine.UI;

class ManaSlider : MonoBehaviour
{
    [SerializeField] private CombatEntity combatEntity;
    [SerializeField] private Slider manaSlider;

    private void Start()
    {
        combatEntity.EventManager.AddListener(
            EventManager.EventType.OnManaChange,
            UpdateMana);

        UpdateMana();
    }

    private void UpdateMana()
    {
        manaSlider.maxValue = combatEntity.MaxMana;
        manaSlider.value = combatEntity.CurrentMana;
    }
}
