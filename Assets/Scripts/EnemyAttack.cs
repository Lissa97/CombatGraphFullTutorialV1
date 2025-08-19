using CombatGraph;
using UnityEngine;

class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float attackRange = 1.5f;

    [SerializeField] Weapon weapon;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private CombatEntity combatEntity;

    [SerializeField] private CombatEntity playerEntity;

    private void Start()
    {
        combatEntity.StartAutoAttack(HandleAttack);
        playerEntity.EventManager.AddListener(
                EventManager.EventType.OnDeath,
                combatEntity.ReloadEntity
            );
    }

    private void HandleAttack(AttackData attackData)
    {
        switch(attackData.AttackName)
        {
            case "SwordHit":
                weapon.Attack(attackData, combatEntity);
                break;

            case "BulletHit":
                Instantiate(bulletPrefab, transform.position, Quaternion.identity)
                    .Initialize(
                        attackData, 
                        combatEntity, 
                        (player.position - transform.position).normalized
                        );
                break;
        }
    }

    private void Update()
    {
        if (Vector2.Distance(player.position, transform.position) > attackRange)
        {
            combatEntity.ProhibitAttack("SwordHit");
            return;
        }

        combatEntity.AllowAttack("SwordHit");
    }
}
