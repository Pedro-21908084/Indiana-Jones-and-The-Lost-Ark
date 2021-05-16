using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : MonoBehaviour
{
    public bool                 inUse = false;
    public bool                 inHookArea = false;
    public Transform            Hook;
    private LineRenderer        whip;
    public Transform            ShootingPoint;
    private DistanceJoint2D     SolidWhip;
    private float               verticalMov;
    public GameObject           whipDamageZone;

    private void Start()
    {
        whip = gameObject.GetComponent<LineRenderer>();
        SolidWhip = gameObject.GetComponent<DistanceJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        verticalMov = Input.GetAxis("Vertical");
        if (inHookArea && !inUse && Input.GetButtonDown("Fire3")) 
        {
            shootwhip();
            gameObject.GetComponent<PlayerMovement>().hooking = true;
        }
        if (Input.GetButtonUp("Fire3")) 
        {
            retractWhip();
            gameObject.GetComponent<PlayerMovement>().hooking = false;
        }
        if (!inUse && Input.GetButtonDown("Fire2")) 
        {
            inUse = true;
            GetComponent<Animator>().SetTrigger("Attack");
        }
        if (inUse) 
        {
            whip.SetPosition(0, ShootingPoint.position);
            whip.SetPosition(1, Hook.position);
        }
        if(SolidWhip.enabled == true) 
        {
            SolidWhip.distance -= verticalMov/10;
        }
    }

    private void shootwhip() 
    {
        inUse = true;
        whip.SetPosition(0, ShootingPoint.position);
        whip.SetPosition(1, Hook.position);
        SolidWhip.anchor = ShootingPoint.position - gameObject.transform.position;
        SolidWhip.connectedAnchor = Hook.position;
        SolidWhip.enabled = true;
        whip.enabled = true;
    }
    private void retractWhip() 
    {
        inUse = false;
        SolidWhip.enabled = false;
        whip.enabled = false;
    }

    //will be call by the animation
    public void WhipAttack() 
    {
        whipDamageZone.gameObject.SetActive(true);
    }
    public void stopAttack() 
    {
        whipDamageZone.gameObject.SetActive(false);
        inUse = false;
    }
}
