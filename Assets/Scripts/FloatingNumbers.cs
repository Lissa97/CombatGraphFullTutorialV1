using TMPro;
using UnityEngine;

class FloatingNumbers : MonoBehaviour
{
    [SerializeField] private float lifeTime = 1f;
    [SerializeField] private float speed = 2f;
    [SerializeField] TMP_Text _text;
    private float spawnTime = 0;

    private void Awake()
    {
        spawnTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - spawnTime > lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += Vector3.up * speed * Time.deltaTime;
        _text.color = new Color(
            _text.color.r, 
            _text.color.g,
            _text.color.b,
            _text.color.a - Time.deltaTime/lifeTime);
    }

    public void SetMessage(string text, Color color)
    {
        _text.text = text;
        _text.color = color;
    }
}
