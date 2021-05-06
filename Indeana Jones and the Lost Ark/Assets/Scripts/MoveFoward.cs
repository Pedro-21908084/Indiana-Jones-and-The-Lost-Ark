using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFoward : MonoBehaviour
{
    public float            speed = 20;
    private Rigidbody2D     Rb;
    void Start()
    {
        Rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //movest the object to its right
        Rb.velocity = gameObject.transform.right * speed;
    }
}
