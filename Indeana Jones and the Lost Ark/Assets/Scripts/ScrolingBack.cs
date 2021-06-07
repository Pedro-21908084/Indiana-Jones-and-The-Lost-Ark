using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrolingBack : MonoBehaviour
{
    public float speed = 0;
    public GameObject player;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //speed = player.GetComponent<Rigidbody2D>().velocity.x / (player.GetComponent<PlayerMovement>().MaxSpeed * player.GetComponent<PlayerMovement>().drag);
        if(player.GetComponent<Rigidbody2D>().velocity.x == 0) 
        {
            speed = 0;
        }else if (player.GetComponent<Rigidbody2D>().velocity.x > 0) 
        {
            speed = 1;
        }else 
        {
            speed = -1;
        }
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(Time.time * speed, 0f);
    }
}
