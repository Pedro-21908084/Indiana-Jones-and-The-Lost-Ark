using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 10;
    public float MaxSpeed = 100;
    public float JumpSpeed = 10;
    public float GravUp = 1;
    public float GravDown = 1.5f;
    public float airDrag = 0.8f;
    public float drag = 1;
    private bool onGround = false;
    private Vector2 Inputs;
    private Rigidbody2D Rb;

    private void Awake()
    {
        Rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        onGround = gameObject.GetComponentInChildren<DetectGroudPoint>().onGround;

        if (!onGround) 
        {
            drag = airDrag;
        }
        else 
        {
            drag = 1;
        }
        
        Inputs.x = Input.GetAxis("Horizontal");

        Rb.velocity = new Vector2(Inputs.x * Speed * drag, Rb.velocity.y);

        if (Rb.velocity.x > MaxSpeed) 
        {
            Rb.velocity = new Vector2(MaxSpeed, Rb.velocity.y);
        }else if (Rb.velocity.x < -MaxSpeed)
        {
            Rb.velocity = new Vector2(-MaxSpeed, Rb.velocity.y);
        }

        CheckGravaty();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && onGround == true)
        {
            Rb.velocity = new Vector2(Rb.velocity.x, JumpSpeed);
        }
    }

    private void CheckGravaty() 
    {
        if (Rb.velocity.y >= 0) 
        {
            Rb.gravityScale = GravUp;
        }else 
        {
            Rb.gravityScale = GravDown;
        }
    }
}
