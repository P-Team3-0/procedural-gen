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
            boss.GetComponent<boss>().playerEntered = true;
            Destroy(gameObject);
        }
    }

}

