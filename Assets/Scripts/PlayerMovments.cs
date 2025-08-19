using CombatGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] 
public class PlayerMovments : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;
    private int isFacingRight = -1;
    [SerializeField] float speed = 5f;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        int direction = (int)Input.GetAxisRaw("Horizontal");

        if(isFacingRight * direction < 0)
        {
            isFacingRight *= -1;
            transform.Rotate(0, 180, 0);
        }   

        playerRigidbody.velocity =
            new Vector2(direction * speed,
            playerRigidbody.velocity.y);
    }
}
