using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWall : MonoBehaviour
{
    public string Floor;
    public bool againstWall = false;

    //when touches the ground bool turns true
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Floor)
        {
            againstWall = true;
        }
    }

    //when leaves the ground bool turns false
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == Floor)
        {
            againstWall = false;
        }
    }
}
