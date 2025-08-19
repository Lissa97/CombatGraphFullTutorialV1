using UnityEngine;

namespace CombatGraphExample2D
{
    public class PlayerMovments : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private Rigidbody2D rb;

        private int direction = -1;

        // Update is called once per frame
        void Update()
        {
            if(Input.GetAxisRaw("Horizontal") != 0)
            {
                if(direction != (int)Input.GetAxisRaw("Horizontal"))
                {
                    direction = (int)Input.GetAxisRaw("Horizontal");
                    transform.Rotate(0, 180, 0);
                }

                Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
                rb.velocity = new Vector2(move.x * speed, rb.velocity.y);
                return;
            }

            rb.velocity = rb.velocity + rb.velocity * Vector2.left;
        }
    }
}


