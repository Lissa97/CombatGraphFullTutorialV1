using UnityEngine;
using CombatGraph;
class Hitter : MonoBehaviour
{
    [SerializeField] private CombatEntity damageTaker;
    public void GetHit(AttackData attackData, CombatEntity damageDealer)
    {
        damageTaker.TakeDamage(attackData, damageDealer);
        Debug.Log(name + " got hit. Current HP: " + damageTaker.CurrentHp);
    }
}
