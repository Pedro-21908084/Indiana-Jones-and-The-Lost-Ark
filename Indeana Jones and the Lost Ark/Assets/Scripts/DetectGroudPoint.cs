using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectGroudPoint : MonoBehaviour
{
    public string Floor;
    public bool onGround = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.tag == Floor) 
        {
            onGround = true;
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == Floor)
        {
            onGround = false;
        }
    }
}
