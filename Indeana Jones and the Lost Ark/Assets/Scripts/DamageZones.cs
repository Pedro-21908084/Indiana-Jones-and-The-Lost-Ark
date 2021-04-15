using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZones : MonoBehaviour
{
    public bool         Continues = false;
    public bool         DestroyOnContact = true;
    public bool         DisableOnContact = false;
    public int          Damage = 0;
    public string       DamagingTag;
    public string       IgnoreTag;
    public string       IgnoreTag2;
    public string   IgnoreTag3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == DamagingTag) 
        {
            collision.GetComponent<Health>().DanmageHeal(-Damage);
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
}
