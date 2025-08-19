using CombatGraph;
using UnityEngine;

class RandomVariationModifier : MonoBehaviour
{
    [SerializeField] private CombatEntity combatEntity;

    private void Start()
    {
        var additionalDefense = Random.Range(0, 100);

        combatEntity.AddNewBuff(
                new DefenseIncrease
                (
                    EventManager.EventType.OnInitialize,
                    AdditionType.Linear,
                    additionalDefense,
                    additionalDefense
                )
            );

        switch (Random.Range(0, 2))
        {
            case 0:
                combatEntity.AddNewBuff(
                    new HealingPersistent
                    (
                        0.4f,
                        1
                    )
                );
                break;

            case 1:
                combatEntity.AddNewBuff(
                    new Invincibility
                    (
                        EventManager.EventType.OnTakeDamage,
                        1.1f,
                        20
                    )
                );
                break;
        }
    }
}
