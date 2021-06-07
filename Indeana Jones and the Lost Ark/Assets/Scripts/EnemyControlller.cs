using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControlller : MonoBehaviour
{
    public float            Range = 20;
    public string           PlayerTag;
    public Transform        eyes;
    private EnemyMovement   Legs;
    private Shooting        Gun;
    public bool             meele = false;
    public int              ShootTimer = 5;
    private float           timer = 0;
    public GameObject       DamageZone;

    // Start is called before the first frame update
    void Start()
    {
        Legs = gameObject.GetComponent<EnemyMovement>();
        if(!meele) 
        {
            Gun = gameObject.GetComponent<Shooting>();
        }
        timer = ShootTimer;
    }

    // Update is called once per frame
    void Update()
    {
        //if the player is in line of ssight and within range stops andattacks
        if (SeePlayer() && timer <= 0) 
        {
            Legs.move = false;
            attack();
            timer = ShootTimer;
        }else if (!SeePlayer())
        {
            Legs.move = true;
        }

        if (timer >= 0) 
        {
            timer -= Time.deltaTime; 
        }
    }

    //cheks if something is in front of it, if it is and it is the player returns true
    bool SeePlayer() 
    {
        Vector2 maxRange = eyes.position + eyes.right * Range;
        RaycastHit2D hit = Physics2D.Linecast(eyes.position, maxRange, 1 << LayerMask.NameToLayer("Interactive"));

        if(hit.collider != null) 
        {
            if (hit.collider.gameObject.CompareTag(PlayerTag))
            {
                return true;
            }
        }
        return false;
    }

    //attacks if it is a range enemy it shoots if not it doers an meele attack
    private void attack() 
    {
        if (meele) 
        {
            GetComponent<Animator>().SetTrigger("Attack");
        }
        else 
        {
            GetComponent<Animator>().SetTrigger("Attack");
            Gun.Shoot = true;
        }
    }

    public void MelleAttack()
    {
        DamageZone.gameObject.SetActive(true);
    }

    public void stopAttack()
    {
        DamageZone.gameObject.SetActive(false);
    }
}
