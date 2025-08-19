using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
class EnemyMovments : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float stoppingDistance = 1f;

    private Rigidbody2D enemyRigidbody;
    private int isFacingRight = 1;

    private void Awake()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (player == null) return;
        if(Vector2.Distance(
            transform.position, player.position) <= stoppingDistance)
        {
            enemyRigidbody.velocity = new Vector2(0f, enemyRigidbody.velocity.y);
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;

        if(isFacingRight * direction.x < 0)
        {
            isFacingRight *= -1;
            transform.Rotate(0, 180, 0);
        }


        enemyRigidbody.velocity =
            new Vector2(direction.x * speed,
            enemyRigidbody.velocity.y);
    }
}

