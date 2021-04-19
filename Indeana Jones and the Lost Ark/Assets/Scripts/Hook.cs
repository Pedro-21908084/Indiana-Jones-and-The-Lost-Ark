using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hook : MonoBehaviour
{
    public TextMeshPro Warn;
    public string PlayerTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == PlayerTag) 
        {
            collision.GetComponent<Whip>().inHookArea = true;
            collision.GetComponent<Whip>().Hook = gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == PlayerTag)
        {
            collision.GetComponent<Whip>().inHookArea = false;
        }
    }
}
