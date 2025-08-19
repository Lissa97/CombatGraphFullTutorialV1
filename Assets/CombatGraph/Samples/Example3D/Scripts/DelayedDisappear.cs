using UnityEngine;

namespace CombatGraphExample3D
{
    public class DelayedDisappear : MonoBehaviour
    {
        [SerializeField] float duration = 0.5f;
        private float timeInit = 0;
        private void Start()
        {
            timeInit = Time.time;
        }

        private void Update()
        {
            if (Time.time - timeInit > duration) Destroy(gameObject);
        }
    }
}


