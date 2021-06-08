using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public bool             Shoot = false;
    public int              Bullets = 5;
    public GameObject       Bullet;
    public GameObject       ShootingPoint;
    public bool             player = true;
    public int              MaxBullets = 3;

    // Update is called once per frame
    void Update()
    {
        //temp
        if (Input.GetButtonDown("Fire1") && player)
        {
            Shoot = true;
        }
        //temp

        if (Shoot && Bullets > 0) 
        {
            if (gameObject.GetComponent<Animator>() != null)
            {
                gameObject.GetComponent<Animator>().SetTrigger("Shoot");
            }
            Instantiate(Bullet, ShootingPoint.transform.position, ShootingPoint.transform.rotation);
            Shoot = false;
            Bullets -= 1;
        }else if (Bullets <= 0) 
        {
            Shoot = false;
        }

        if(player && Bullets > MaxBullets) 
        {
            Bullets = MaxBullets;
        }
    }
}
