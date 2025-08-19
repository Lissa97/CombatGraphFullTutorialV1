using CombatGraph;
using UnityEngine;
using UnityEngine.UI;
class HPSlider : MonoBehaviour
{
    [SerializeField] private CombatEntity combatEntity;
    [SerializeField] private Slider hpSlider;

    private void Start()
    {
        combatEntity.EventManager.AddListener(
            EventManager.EventType.OnHpChange,
            UpdateHP);

        UpdateHP();
    }

    private void UpdateHP()
    {
        hpSlider.maxValue = combatEntity.MaxHp;
        hpSlider.value = combatEntity.CurrentHp;
    }
}
