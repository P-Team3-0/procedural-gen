using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public int health;

    private bool playerTakeDamage = true;
    public void TakeDamage(int damage)
    {
        if (gameObject.tag == "Player")
        {
            if (playerTakeDamage)
            {
                health -= damage;
                Debug.Log("Health: " + health);
                playerTakeDamage = false;
                Invoke(nameof(ResetTakeDamage), 5);
            }
        }
        else
        {
            health -= damage;
            Debug.Log("Health: " + health);
        }
    }
    private void ResetTakeDamage()
    {
        playerTakeDamage = true;
    }
}
