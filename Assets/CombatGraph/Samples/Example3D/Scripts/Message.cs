using UnityEngine.UI;
using UnityEngine;

namespace CombatGraphExample3D
{
    public class Message : MonoBehaviour
    {
        [SerializeField] Text mesageText;
        [SerializeField] Canvas canvas;
        private float appearTime = 0;

        private void Start()
        {
            appearTime = Time.time;
            canvas.worldCamera = Camera.main;
        }

        private void Update()
        {
            if(Time.time - appearTime > 4) Destroy(gameObject);

            transform.position += Vector3.up * Time.deltaTime * 3;
        }

        public void UpdateText(string text, Color color)
        {
            mesageText.text = text;
            mesageText.color = color;
        }
    }
}


