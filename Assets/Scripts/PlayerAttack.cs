using CombatGraph;
using UnityEngine;

class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private Bullet bullet;
    [SerializeField] private CombatEntity combatEntity;

    private void Update()
    {
        if (Input.GetKey("x"))
        {
            var attackData = combatEntity.Attack("SwordHit");
            if (!attackData.IsAvalible) return;

            weapon.Attack(attackData, combatEntity);
        }

        if(Input.GetKey("y"))
        {
            var attackData = combatEntity.Attack("BulletHit");
            if(!attackData.IsAvalible) return;

            Instantiate(bullet, transform.position, Quaternion.identity)
                .Initialize(
                    attackData,
                    combatEntity,
                    Mathf.Abs(transform.eulerAngles.y % 360 - 180) < 1 ? 
                    Vector3.right : Vector3.left
                );
        }
    }
}
