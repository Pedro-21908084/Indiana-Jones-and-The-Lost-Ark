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
    private Vector2 Inputs;
    private Rigidbody2D Rb;

    private void Awake()
    {
        Rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Inputs.x = Input.GetAxis("Horizontal");

        Rb.velocity = new Vector2(Inputs.x * Speed, Rb.velocity.y);

        if (Rb.velocity.x > MaxSpeed) 
        {
            Rb.velocity = new Vector2(MaxSpeed, Rb.velocity.y);
        }else if (Rb.velocity.x < -MaxSpeed)
        {
            Rb.velocity = new Vector2(-MaxSpeed, Rb.velocity.y);
        }

        checkGravaty();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Inputs.y = 1;
            Rb.velocity = new Vector2(Rb.velocity.x, JumpSpeed);
        }
        else 
        {
            Inputs.y = 0;
        }
    }

    private void checkGravaty() 
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
