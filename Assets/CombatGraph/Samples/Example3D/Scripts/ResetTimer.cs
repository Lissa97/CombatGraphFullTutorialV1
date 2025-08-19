using UnityEngine;
using CombatGraph;
using UnityEngine.UI;

namespace CombatGraphExample3D
{
    public class ResetTimer : MonoBehaviour
    {
        [SerializeField] CombatEntity combatEntity;
        [SerializeField] string attackName;
        [SerializeField] Text timerText;
        private void Update()
        {
            if (combatEntity.GetTimer(attackName).TimeTillReset == 0)
            {
                timerText.text = attackName + " is ready!";
                return;
            }

            timerText.text = attackName + " will be ready in " + combatEntity.GetTimer(attackName).TimeTillReset.ToString();
        }
    }
}


