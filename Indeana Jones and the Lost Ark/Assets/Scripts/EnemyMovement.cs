using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject Left;
    private float MaxLeft = 0;
    public GameObject Right;
    private float MaxRight = 0;
    public float Speed = 10;
    public float MaxSpeed = 100;
    private bool onGround = false;
    private bool toTheLeft = true;
    private Rigidbody2D Rb;

    private void Start()
    {
        if (Left != null)
        {
            MaxLeft = Left.transform.position.x;

        }
        if (Right != null)
        {
            MaxRight = Right.transform.position.x;
        }

        Rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        onGround = gameObject.GetComponentInChildren<DetectGroudPoint>().onGround;

        if(toTheLeft && onGround && gameObject.transform.position.x > MaxLeft) 
        {
            Rb.velocity = new Vector2(-Speed, Rb.velocity.y);
        }else if (!toTheLeft && onGround && gameObject.transform.position.x < MaxRight)
        {
            Rb.velocity = new Vector2(Speed, Rb.velocity.y);
        }

        if (gameObject.transform.position.x <= MaxLeft) 
        {
            toTheLeft = false;
        }else if (gameObject.transform.position.x >= MaxRight)
        {
            toTheLeft = true;
        }
    }
}
