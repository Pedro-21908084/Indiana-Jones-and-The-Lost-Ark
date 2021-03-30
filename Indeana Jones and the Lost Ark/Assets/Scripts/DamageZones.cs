using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZones : MonoBehaviour
{
    public bool Continues = false;
    public bool DestroyOnContact = true;
    public int Damage = 0;
    public string DamagingTag;
    public string IgnoreTag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == DamagingTag) 
        {
            collision.GetComponent<Health>().DanmageHeal(-Damage);
        }
        if (DestroyOnContact && collision.tag != IgnoreTag) 
        {
            Destroy(gameObject);
        }
    }
}
