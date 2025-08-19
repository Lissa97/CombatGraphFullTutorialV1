using UnityEngine;
using UnityEngine.UI;
using CombatGraph;

namespace CombatGraphExample2D
{
    class HPSlider : MonoBehaviour
    {
        [SerializeField] CombatEntity combatEntity;
        [SerializeField] Slider hpSlider;
        [SerializeField] Text hpText;

        private void Start()
        {
            combatEntity.EventManager.AddListener(EventManager.EventType.OnHpChange, UpdateField);
            UpdateField();
        }

        private void UpdateField()
        {
            hpSlider.maxValue = combatEntity.MaxHp;
            hpSlider.value = combatEntity.CurrentHp;

            hpText.text = $"{combatEntity.CurrentHp}/{combatEntity.MaxHp}";
        }

        private void OnDestroy()
        {
            combatEntity?.EventManager.RemoveListener(EventManager.EventType.OnHpChange, UpdateField);   
        }
    }
}


