using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //game objects and floats to store the ends of wlak cicles
    public GameObject       Left;
    private float           MaxLeft = 0;
    public GameObject       Right;
    private float           MaxRight = 0;
    public bool             move = true;
    public float            Speed = 10;
    public float            MaxSpeed = 100;
    private bool            onGround = false;
    private bool            toTheLeft = true;
    private Rigidbody2D     Rb;

    private void Start()
    {
        //stores the inicial position of the gameobjects in the corresponding floats, if there is no gameobjects the floats stay = 0
        if (Left != null)
        {
            MaxLeft = Left.transform.position.x;

        }
        if (Right != null)
        {
            MaxRight = Right.transform.position.x;
        }

        //assigns the gameobjects rigidbody into a variable
        Rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //verefys if it is on the ground
        onGround = gameObject.GetComponentInChildren<DetectGroudPoint>().onGround;

        //if it is on the ground, is going left and its position didnt pass the max point keeps the -speed else its speed becames +
        if(toTheLeft && onGround && gameObject.transform.position.x > MaxLeft && move) 
        {
            Rb.velocity = new Vector2(-Speed, Rb.velocity.y);
        }else if (!toTheLeft && onGround && gameObject.transform.position.x < MaxRight && move)
        {
            Rb.velocity = new Vector2(Speed, Rb.velocity.y);
        }else if (move == false) 
        {
            Rb.velocity = new Vector2(0, Rb.velocity.y);
        }

        //if it reaches the max point turns to the other side
        if (gameObject.transform.position.x <= MaxLeft) 
        {
            toTheLeft = false;
        }else if (gameObject.transform.position.x >= MaxRight)
        {
            toTheLeft = true;
        }
    }

    private void Update()
    {
        if (Rb.velocity.x > 0)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
}
