using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBallMovement : ProjectileMove
{
    private Vector3 direction;

    public GameObject fire;

    public int fireBallDamage;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Floor"))
        {
            Instantiate(fire, other.transform.position, Quaternion.identity);
        }
        if (other.CompareTag("Player"))
        {
            other.GetComponent<LifeManager>().TakeDamage(fireBallDamage);
        }
        Destroy(gameObject);
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    public void SetSpeed(float distanceFromPlayer)
    {
        speed = distanceFromPlayer * speed;
    }

}