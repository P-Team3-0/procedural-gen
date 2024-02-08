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
    GameObject DeathEffect;
    void Start()
    {
        Bubble = GameObject.FindWithTag("Bubble");
        DeathEffect = GameObject.FindWithTag("Death");

        if (Bubble != null && DeathEffect != null)
        {
            Bubble.SetActive(false);
            DeathEffect.SetActive(false);
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
        if (Bubble != null && DeathEffect != null)
        {
            Bubble.SetActive(false);
            DeathEffect.SetActive(false);
        }
    }
    private void Die()
    {
        if (Bubble != null && DeathEffect !=null)
        {
            Bubble.SetActive(false);
            DeathEffect.SetActive(true);
        }
        isDead = true;
        animator.SetTrigger("Death");
        GetComponent<RigidbodyMovement>().enabled = false;
        GetComponent<LifeManager>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().isTrigger = true;
        GameObject firePoint = gameObject.transform.Find("Sphere").gameObject;
        firePoint.GetComponent<WitchAttack>().enabled = false;
    }
}
