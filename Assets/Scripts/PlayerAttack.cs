using CombatGraph;
using UnityEngine;

class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private float delayTime = 0.5f;

    private float lastAttackTime = 0f;
    private void Update()
    {
        if (Time.time - lastAttackTime < delayTime)
        {
            return;
        }

        if (Input.GetKey("x"))
        {
            lastAttackTime = Time.time;
            weapon.Attack();
        }
    }
}