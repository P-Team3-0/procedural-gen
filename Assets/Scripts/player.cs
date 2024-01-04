using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //Player variables
    public int health;
    public int attack;

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
        Debug.Log("Player health: " + health);
        if (health <= 0)
        {
            // Debug.Log("Player died");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Goblin")
        {
            collision.gameObject.GetComponent<enemy>().TakeDamage(attack);
            // Reduce the impact of collision 
            // Vector3 force = -collision.relativeVelocity * 0.7f;
            // GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }
    }
}