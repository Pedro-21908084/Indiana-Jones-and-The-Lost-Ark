using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //public float            Speed = 10;
    public float            MaxSpeed = 10;
    public float            JumpSpeed = 10;
    public float            GravUp = 1;
    public float            GravDown = 1.5f;
    public float            airDrag = 0.8f;
    public float            drag = 1;
    private bool            onGround = false;
    private Vector2         Inputs;
    private Rigidbody2D     Rb;
    public bool             hooking = false;

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
        if (hooking)
        {
            Rb.velocity = new Vector2(Rb.velocity.x + Inputs.x * drag, Rb.velocity.y);
            if (Rb.velocity.x > MaxSpeed)
            {
                if (Rb.velocity.x > 2 * MaxSpeed)
                {
                    Rb.velocity = new Vector2(2 * MaxSpeed, Rb.velocity.y);
                }
                else if (Rb.velocity.x > 2 * MaxSpeed)
                {
                    Rb.velocity = new Vector2(2 * MaxSpeed, Rb.velocity.y);
                }
            }
        }else
        {
            Rb.velocity = new Vector2(MaxSpeed * Inputs.x * drag, Rb.velocity.y);
        }

        if (GetComponent<Whip>().attacking) 
        {
            Rb.velocity = new Vector2(0, Rb.velocity.y);
        }
                /*if (Inputs.x != 0)
                {
                    Rb.velocity = new Vector2(Rb.velocity.x + Inputs.x * drag, Rb.velocity.y);
                    if(Rb.velocity.x > MaxSpeed) 
                    {
                        if (hooking) 
                        {
                            if(Rb.velocity.x > 2 * MaxSpeed) 
                            {
                                Rb.velocity = new Vector2(2 * MaxSpeed, Rb.velocity.y);
                            }
                        }
                        else
                        {
                            Rb.velocity = new Vector2(MaxSpeed, Rb.velocity.y);
                        }

                    }else if (Rb.velocity.x < -MaxSpeed)
                    {
                        if (hooking)
                        {
                            if (Rb.velocity.x < 2 * -MaxSpeed)
                            {
                                Rb.velocity = new Vector2(2 * -MaxSpeed, Rb.velocity.y);
                            }
                        }
                        else
                        {
                            Rb.velocity = new Vector2(-MaxSpeed, Rb.velocity.y);
                        }
                    }
                }*/

                CheckGravaty();
    }

    void Update()
    {
        GetComponent<Animator>().SetBool("IsJumping", !onGround);
        GetComponent<Animator>().SetFloat("speed", Mathf.Abs(Rb.velocity.x));
        //if the object is on ground and the space key is pressed the object jumps
        if (Input.GetButtonDown("Jump") && onGround == true)
        {
            Rb.velocity = new Vector2(Rb.velocity.x, JumpSpeed);
        }

        if (Rb.velocity.x > 0.2 && !hooking)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        }else if(Rb.velocity.x < -0.2 && !hooking)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (Inputs.x == 0 && !hooking && onGround)
        {
            Rb.velocity = new Vector2(0, Rb.velocity.y);
        }
    }

    //changes the gravaty scale depending if the object is falling or going up
    private void CheckGravaty() 
    {
        if (Rb.velocity.y >= 0) 
        {
            Rb.gravityScale = GravUp;
        }else if(!hooking)
        {
            Rb.gravityScale = GravDown;
        }

        if(Rb.velocity.y > JumpSpeed) 
        {
            Rb.velocity = new Vector2(Rb.velocity.x, JumpSpeed);
        }
    }
}
