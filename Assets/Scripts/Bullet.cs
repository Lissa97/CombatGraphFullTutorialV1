using CombatGraph;
using UnityEngine;

class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private LayerMask targetLayer;

    private float _spawnTime;
    private Vector3 _direction;
    private AttackData _attackData;
    private CombatEntity _damageDealer;

    private void FixedUpdate()
    {
        if (Time.time - _spawnTime > lifeTime)
        {
            Destroy(gameObject);
            return;
        }
        transform.Translate(_direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer.value) == 0) return;

        var hitter = collision.GetComponent<Hitter>();

        if (hitter == null) return;

        hitter.GetHit(_attackData, _damageDealer);
    }

    public void Initialize(
        AttackData attackData, 
        CombatEntity damageDealer,
        Vector3 direction
        )
    {
        _attackData = attackData;
        _damageDealer = damageDealer;
        _direction = direction;

        _spawnTime = Time.time;
    }
}