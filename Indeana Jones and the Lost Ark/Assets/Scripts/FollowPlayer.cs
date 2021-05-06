using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject   Player;
    public float        radius = 5;
    private Vector3     distanceToPlayer;

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Player.transform.position - gameObject.transform.position;

        if (distanceToPlayer.x > radius) 
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + (distanceToPlayer.x - radius), gameObject.transform.position.y, gameObject.transform.position.z);
        }else if(distanceToPlayer.x < -radius) 
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + (distanceToPlayer.x + radius), gameObject.transform.position.y, gameObject.transform.position.z);
        }

        if (distanceToPlayer.y > radius)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (distanceToPlayer.y - radius), gameObject.transform.position.z);
        }
        else if (distanceToPlayer.y < -radius)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (distanceToPlayer.y + radius), gameObject.transform.position.z);
        }
    }
}
