using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int HP = 1;
    public GameObject drop;
    public bool invensible = false;
    public int invensibleTimer = 5;
    private float timer = 0;
    public bool player = false;
    public GameObject restart;

    // Update is called once per frame
    void Update()
    {
        if(HP <= 0) 
        {
            //play animation first
            Death();
        }
        if(timer > 0) 
        {
            invensible = true;
            timer -= Time.deltaTime;
        }
        else 
        {
            invensible = false;
        }
    }

    public void DanmageHeal(int mod) 
    {
        if (!invensible)
        {
            HP += mod;
            if (mod < 0)
            {
                timer = invensibleTimer;
            }
        }
        
    }

    private void Death() 
    {
        if(drop != null) 
        {
            Instantiate(drop, gameObject.transform.position, gameObject.transform.rotation);
        }
        if (player) 
        {
            //Destroy(gameObject);
            restart.SetActive(true);
            gameObject.SetActive(false);
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}
