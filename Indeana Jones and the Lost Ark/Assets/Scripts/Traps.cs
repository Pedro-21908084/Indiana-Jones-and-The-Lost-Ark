using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public bool falseFloor = true;
    public bool arrows = false;
    public bool spearsGround = false;
    public GameObject spears;
    public bool rocks = false;
    public string   target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == target)
        {
            if (falseFloor)
            {
                ActivateFalseFloor();
            }
            else if (arrows)
            {
                ActivateArrows();
            }
            else if (spearsGround)
            {
                ActivateSpears();
            }
            else if (rocks)
            {
                ActivateRocks();
            }
        }
    }

    private void ActivateFalseFloor() 
    {
        gameObject.SetActive(false);
    }

    private void ActivateArrows()
    {
        GetComponent<Shooting>().Shoot = true;
    }

    private void ActivateSpears()
    {
        spears.SetActive(true);
    }

    private void ActivateRocks()
    {
        gameObject.SetActive(false);
    }

}
