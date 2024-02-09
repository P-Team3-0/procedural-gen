using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject boss = GameObject.FindWithTag("Boss");
            GameObject player = GameObject.FindWithTag("Player");
            boss.GetComponent<boss>().playerEntered = true;
            player.GetComponent<RigidbodyMovement>().playerEntered = true;
            Destroy(gameObject);
        }
    }
}

