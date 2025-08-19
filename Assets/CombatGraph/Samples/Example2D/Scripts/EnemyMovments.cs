using UnityEngine;

namespace CombatGraphExample2D
{
    public class EnemyMovments : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Transform player;

        private bool inMoving = true;
        private int direction = 1;

        // Update is called once per frame
        void Update()
        {
            if (!inMoving) return;

            if (player.transform.position.x > transform.position.x)
            {
                if (direction != 1)
                {
                    direction = 1;
                    transform.Rotate(0, 180, 0);
                }
            }
            else if(direction != -1)
            {
                direction = -1;
                transform.Rotate(0, 180, 0);
            }

            if (Vector3.Distance(player.transform.position, transform.position) > 4)
            {
                Vector3 move = new Vector3(direction, 0, 0);
                rb.velocity = new Vector2(move.x * speed, rb.velocity.y);

                return;
            }

            rb.velocity = rb.velocity + rb.velocity * Vector2.left;
        }

        public void Move()
        {
            inMoving = true;
        }

        public void Stop()
        {
            inMoving = false;
        }

    }
}


