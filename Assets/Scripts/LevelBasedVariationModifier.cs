using CombatGraph;
using UnityEngine;

class LevelBasedVariationModifier : MonoBehaviour
{
    [SerializeField] private CombatEntity combatEntity;
    [SerializeField] private int level = 1;

    private int additionalDefense
    {
        get
        {
            return 50 + level * 50;
        }
    }

    private int additionalHp
    {
        get
        {
            return level * 30;
        }
    }

    private void Start()
    {
        combatEntity.AddNewBuff(
                new DefenseIncrease
                (
                    EventManager.EventType.OnInitialize,
                    AdditionType.Linear,
                    additionalDefense,
                    additionalDefense
                )
            );

        combatEntity.AddNewBuff(
            new MaxHPIncrease
            (
                EventManager.EventType.OnInitialize,
                AdditionType.PercentageBased,
                additionalHp,
                additionalHp
            )
        );
    }
}