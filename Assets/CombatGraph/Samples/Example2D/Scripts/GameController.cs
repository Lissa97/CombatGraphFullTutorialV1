using CombatGraph;
using UnityEngine;
using UnityEngine.UI;

namespace CombatGraphExample2D
{
    class GameController : MonoBehaviour
    {
        [SerializeField] CombatEntity playerEntity;
        [SerializeField] CombatEntity enemyEntity;

        [SerializeField] GameObject blessingSelection;
        [SerializeField] GameObject combatEnd;
        [SerializeField] Text winner;
        private void Start()
        {
            Time.timeScale = 0;

            playerEntity.EventManager.AddListener(EventManager.EventType.OnDeath, () => DisplayWinner("Moster"));
            enemyEntity.EventManager.AddListener(EventManager.EventType.OnDeath, () => DisplayWinner("Player"));
        }

        private void DisplayWinner(string winnerName)
        {
            Time.timeScale = 0;
            winner.text = winnerName + " wins!";
            combatEnd.SetActive(true);
        }

        public void SelectBlessing(int id)
        {
            if (id == 1)
            {
                playerEntity.AddNewBuff(new DodgeRateIncrease(
                    EventManager.EventType.OnInitialize,
                    40,
                    40));
                Time.timeScale = 1;
                blessingSelection.SetActive(false);
                return;
            }

            if (id == 2)
            {
                playerEntity.AddNewBuff(new DamageIncrease(
                    EventManager.EventType.OnManaSpent,
                    AdditionType.Linear,
                    5,
                    20));

                Time.timeScale = 1;
                blessingSelection.SetActive(false);

                return;
            }
        }

        public void Restart()
        {
            Time.timeScale = 0;
            blessingSelection.SetActive(true);
            combatEnd.SetActive(false);

            playerEntity.ReloadEntity();
            enemyEntity.ReloadEntity();
            enemyEntity.StartAutoAttack();
        }
    }
}


