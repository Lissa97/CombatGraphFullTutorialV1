using CombatGraph;
using UnityEngine;

class InfiniteGun : MonoBehaviour
{
    [SerializeField] private CombatEntity combatEntity;
    [SerializeField] private Bullet bulletPrefab;

    private void Start()
    {
        combatEntity.StartAutoAttack(AttackHandler);
    }

    private void AttackHandler(AttackData attackData)
    {
        switch (attackData.AttackName)
        {
            case "SwordHit":
                Instantiate(bulletPrefab, transform.position, Quaternion.identity)
                    .Initialize(attackData, combatEntity, Vector2.right);
                break;
        }
    }
}
