using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //Player variables
    public int health;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        Debug.Log(health);
        if (health <= 0)
        {
            Debug.Log("Player died");
        }
    }
}