using CombatGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
class Weapon : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private SpriteRenderer spriteRenderer;

    HashSet<Hitter> hitters = new();

    private void Awake()
    {
        spriteRenderer.enabled = false;
    }

    internal void Attack(AttackData attackData, CombatEntity damageDealer)
    {
        StartCoroutine(AttackWithDelay(attackData, damageDealer));
    }

    IEnumerator AttackWithDelay(AttackData attackData, CombatEntity damageDealer)
    {
        foreach (var hitter in hitters)
        {
            hitter.GetHit(attackData, damageDealer);
        }

        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayer.value) == 0) return;

        var hitter = collision.GetComponent<Hitter>();

        if(hitter == null) return;

        hitters.Add(hitter);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & enemyLayer.value) == 0) return;

        var hitter = collision.GetComponent<Hitter>();

        if (hitter == null) return;

        hitters.Remove(hitter);
    }


}

