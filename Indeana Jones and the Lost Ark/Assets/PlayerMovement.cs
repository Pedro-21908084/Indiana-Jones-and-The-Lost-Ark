using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 10;
    public float MaxSpeed = 100;
    private Vector2 Inputs;
    private Rigidbody2D Rb;

    private void Awake()
    {
        Rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Inputs.x = Input.GetAxis("Horizontal");

        //Debug.Log(Inputs.x);

        Rb.velocity = Inputs * Speed;

        if(Rb.velocity.x > MaxSpeed) 
        {
            Rb.velocity = new Vector2(MaxSpeed, Rb.velocity.y);
        }else if (Rb.velocity.x < -MaxSpeed)
        {
            Rb.velocity = new Vector2(-MaxSpeed, Rb.velocity.y);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
