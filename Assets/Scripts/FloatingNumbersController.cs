using CombatGraph;
using UnityEngine;

class FloatingNumbersController : MonoBehaviour
{
    [SerializeField] private CombatEntity combatEntity;
    [SerializeField] private FloatingNumbers floatingNumbersPrefab;

    private void Start()
    {
        combatEntity.EventManager.AddListener<DamageData>
            (
                EventManager.EventType.OnTakeDamage,
                (damageData) =>
                {
                    Instantiate(
                            floatingNumbersPrefab, 
                            combatEntity.transform.position, 
                            Quaternion.identity
                        )
                        .SetMessage(
                            damageData.DamageTaken.ToString(),
                            damageData.Status == DamageStatus.Critical ?
                            Color.red : Color.white
                        );
                }
            );

        combatEntity.EventManager.AddListener<int>(
            EventManager.EventType.OnHeal,
            (healAmount) =>
            {
                Instantiate(
                    floatingNumbersPrefab,
                    combatEntity.transform.position,
                    Quaternion.identity
                )
                .SetMessage(
                    "+" + healAmount,
                    Color.green
                );
            }
        );
    }
}