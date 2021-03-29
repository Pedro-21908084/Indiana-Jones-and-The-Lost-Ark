using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float            Speed = 10;
    public float            JumpSpeed = 10;
    public float            GravUp = 1;
    public float            GravDown = 1.5f;
    public float            airDrag = 0.8f;
    public float            drag = 1;
    private bool            onGround = false;
    private Vector2         Inputs;
    private Rigidbody2D     Rb;

    private void Awake()
    {
        //assigns the gameobjects rigidbody into a variable
        Rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //verefys if it is on the ground
        onGround = gameObject.GetComponentInChildren<DetectGroudPoint>().onGround;

        //changes the drag depending if the object is on the ground or not
        if (!onGround) 
        {
            drag = airDrag;
        }
        else 
        {
            drag = 1;
        }
        
        //gets the input of the x axis , keys a and d
        Inputs.x = Input.GetAxis("Horizontal");

        //varies the velocity of the object depending on the input
        Rb.velocity = new Vector2(Inputs.x * Speed * drag, Rb.velocity.y);

        CheckGravaty();
    }

    void Update()
    {
        //if the object is on ground and the space key is pressed the object jumps
        if (Input.GetButtonDown("Jump") && onGround == true)
        {
            Rb.velocity = new Vector2(Rb.velocity.x, JumpSpeed);
        }
    }

    //changes the gravaty scale depending if the object is falling or going up
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
