using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZones : MonoBehaviour
{
    public bool         Continues = false;
    private GameObject  target;
    public bool         DestroyOnContact = true;
    public bool         DisableOnContact = false;
    public int          Damage = 0;
    public string       DamagingTag;
    public string       DamagingTag2;
    public string       IgnoreTag;
    public string       IgnoreTag2;
    public string       IgnoreTag3;

    public void Update()
    {
        if(target != null) 
        {
            target.GetComponent<Health>().DanmageHeal(-Damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == DamagingTag || collision.tag == DamagingTag2) 
        {
            collision.GetComponent<Health>().DanmageHeal(-Damage);
            if (DestroyOnContact) 
            {
                Destroy(gameObject);
            }else if (DisableOnContact) 
            {
                gameObject.SetActive(false);
            }
            if (Continues) 
            {
                target = collision.gameObject;
            }
        }

        if (DestroyOnContact && collision.tag != IgnoreTag && collision.tag != IgnoreTag2 && collision.tag != IgnoreTag3) 
        {
            Destroy(gameObject);
        }
        else if (DisableOnContact && collision.tag != IgnoreTag && collision.tag != IgnoreTag2 && collision.tag != IgnoreTag3)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Continues || target != null)
        {
            if (collision.gameObject.name == target.name)
            {
                target = null;
            }
        }
    }
}
