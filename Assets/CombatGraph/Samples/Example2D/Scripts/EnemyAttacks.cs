using CombatGraph;
using System.Collections;
using UnityEngine;

namespace CombatGraphExample2D
{
    class EnemyAttacks : MonoBehaviour
    {
        [SerializeField] CombatEntity combatEntity;
        [SerializeField] EnemyMovments enemyMovments;
        [SerializeField] Transform player;

        private void Start()
        {
            combatEntity.StartAutoAttack(AttacksHandler);   
        }

        void AttacksHandler(AttackData attackData)
        {
            switch (attackData.AttackName)
            {
                case "Basic Attack":
                    BasicAttack(attackData);
                    break;

                case "Mana Attack":
                    ManaAttack(attackData);
                    break;
            }
        }
        [SerializeField] Weapon skyFireBalls;
        void ManaAttack(AttackData attackData)
        {
            StartCoroutine(ManaAttackAnimations(attackData));
        }

        IEnumerator ManaAttackAnimations(AttackData attackData)
        {
            enemyMovments.Stop();
            yield return new WaitForSeconds(0.5f);
            Weapon newWeapon;

            for (int i = 0; i < 3; i++)
            {
                newWeapon = Instantiate(skyFireBalls);
                newWeapon.transform.position += Vector3.left * i * 10;
                newWeapon.SubscribeOnHitAction((entity) => entity.TakeDamage(attackData, combatEntity));

                newWeapon = Instantiate(skyFireBalls);
                newWeapon.transform.position -= Vector3.left * i * 10;
                newWeapon.SubscribeOnHitAction((entity) => entity.TakeDamage(attackData, combatEntity));
            }

            yield return new WaitForSeconds(0.5f);
            enemyMovments.Move();
        }

        [SerializeField] Weapon sword;
        void BasicAttack(AttackData attackData)
        {
            Instantiate(sword, transform.position, transform.rotation, transform)
                .SubscribeOnHitAction((entity) => { entity.TakeDamage(attackData, combatEntity); });
        }

        private void Update()
        {
            if (Vector2.Distance(player.position, transform.position) > 6)
            {
                combatEntity.ProhibitAttack("Basic Attack");
            }
            else
            {
                combatEntity.AllowAttack("Basic Attack");
            }
        }
    }
}


