using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public int health;
    Animator animator;
    private bool isDead=false;
    private bool playerTakeDamage = true;
    public void TakeDamage(int damage)
    {
        if (gameObject.tag == "Player")
        {
            animator = GetComponent<Animator>();
            if (playerTakeDamage)
            {
                animator.SetTrigger("Colpo");
                health -= damage;
                Debug.Log("Health: " + health);
                if (health <= 0 && !isDead)
                {
                    Die();
                }
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
    private void Die()
    {
        // Imposta la variabile isDead su true
        isDead = true;

        // Attiva il trigger dell'animazione di morte
        animator.SetTrigger("Death");
        GetComponent<RigidbodyMovement>().enabled = false;
        GetComponent<LifeManager>().enabled = false;
    }
}
