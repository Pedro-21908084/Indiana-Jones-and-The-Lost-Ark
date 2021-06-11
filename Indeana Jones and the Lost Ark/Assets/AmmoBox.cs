using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int Ammo = 0;
    public string Player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Player) 
        {
            collision.GetComponent<Shooting>().Bullets += Ammo;
            Destroy(gameObject);
        }
    }
}
