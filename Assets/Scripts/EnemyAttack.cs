using CombatGraph;
using UnityEngine;

class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float attackRange = 1.5f;

    [SerializeField] Weapon weapon;
    [SerializeField] private float delayTime = 0.5f;

    private float lastAttackTime = 0f;

    private void Update()
    {
        if (Vector2.Distance(player.position, transform.position) > attackRange)
        {
            return;
        }
        if (Time.time - lastAttackTime < delayTime)
        {
            return;
        }
        lastAttackTime = Time.time;
        weapon.Attack();
    }
}