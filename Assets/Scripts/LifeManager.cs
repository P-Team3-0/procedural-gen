using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public int health;
    Animator animator;
    private bool isDead=false;
    private bool playerTakeDamage = true;
    GameObject Bubble;
    void Start()
    {
        Bubble = GameObject.FindWithTag("Bubble");

        if (Bubble != null)
        {
            Bubble.SetActive(false);
        }
    }
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
                if (health>0 && Bubble != null)
                {
                    Bubble.SetActive(true);
                }
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
        if (Bubble != null)
        {
            Bubble.SetActive(false);
        }
    }
    private void Die()
    {
        if (Bubble != null)
        {
            Bubble.SetActive(false);
        }
        isDead = true;
        animator.SetTrigger("Death");
        GetComponent<RigidbodyMovement>().enabled = false;
        GetComponent<LifeManager>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().isTrigger = true;
    }
}
