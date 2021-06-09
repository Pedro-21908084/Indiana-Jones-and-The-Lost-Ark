using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class range : MonoBehaviour
{
    public string PlayerTag;
    public bool Boss = false;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == PlayerTag)
        {
            float distplayyer = collision.transform.position.x - gameObject.transform.position.x;
            if (!Boss && isLookingPlayer(distplayyer))
            {
                gameObject.GetComponentInParent<EnemyControlller>().SeePlayer = true;
            }
            else if (!Boss)
            {
                gameObject.GetComponentInParent<EnemyControlller>().SeePlayer = false;
            }

            if (Boss)
            {
                if (!isLookingPlayer(distplayyer))
                {
                    if (gameObject.transform.eulerAngles.y == 180)
                    {
                        gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
                    }
                }
                gameObject.GetComponentInParent<EnemyControlller>().SeePlayer = true;
            }
        }
        else
        {
            gameObject.GetComponentInParent<EnemyControlller>().SeePlayer = false;
        }
    }

    private bool isLookingPlayer(float dist)
    {
        if (dist == Mathf.Abs(dist) * gameObject.transform.right.x)
        {
            return true;
        }
        return false;
    }
}
