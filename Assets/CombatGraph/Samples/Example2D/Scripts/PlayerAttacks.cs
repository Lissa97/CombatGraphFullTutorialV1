using CombatGraph;
using UnityEngine;

namespace CombatGraphExample2D
{
    public class PlayerAttacks : MonoBehaviour
    {
        [SerializeField] CombatEntity combatEntity;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                BasicAttack(combatEntity.Attack("Basic Attack"));
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                ManaAttack(combatEntity.Attack("Mana Attack"));
            }
        }

        [SerializeField] Weapon manaBlust;

        void ManaAttack(AttackData data)
        {
            if (!data.IsAvalible)
            {
                Debug.Log("Attack is unavailable!");
                return;
            }

            Instantiate(manaBlust, transform.position, transform.rotation)
                .SubscribeOnHitAction((entity) => { entity.TakeDamage(data, combatEntity); });
        }

        [SerializeField] Weapon sword;
        void BasicAttack(AttackData data)
        {
            if (!data.IsAvalible)
            {
                Debug.Log("Attack is inavalible!");
                return;
            }

            Instantiate(sword, transform.position, transform.rotation, transform)
                .SubscribeOnHitAction((entity) => { entity.TakeDamage(data, combatEntity); });
        }
    }

}


