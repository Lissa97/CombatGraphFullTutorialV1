using CombatGraph;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class SkillCooldownDisplay : MonoBehaviour
{
    [SerializeField] private CombatEntity combatEntity;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private Slider coldownSlider;
    [SerializeField] private string attackName;

    private void Update()
    {
        coldownSlider.maxValue = combatEntity.GetTimer(attackName).CooldownTime;
        coldownSlider.value = combatEntity.GetTimer(attackName).TimeTillReset;

        timeText.text = ""
            + combatEntity.GetTimer(attackName).TimeTillResetInSeconds 
            + "/"
            + combatEntity.GetTimer(attackName).CooldownTimeInSeconds;
    }
}