using UnityEngine;

namespace CombatGraphExample3D
{
    public class ForwardMovments : MonoBehaviour
    {
        [SerializeField] float speed = 20;
        private float timeInit = 0;
        private void Start()
        {
            timeInit = Time.time;
        }

        private void Update()
        {
            if (Time.time - timeInit > 6) Destroy(gameObject);

            transform.position -= transform.right * speed * Time.deltaTime;
        }
    }
}


