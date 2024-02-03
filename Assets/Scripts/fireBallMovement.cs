using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBallMovement : ProjectileMove
{
    public Vector3 direction;

    public GameObject fire;

    public int fireBallDamage;

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);
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
        if (direction.z < 0)
        {
            distanceFromPlayer = -distanceFromPlayer;
        }
        speed = distanceFromPlayer * speed;
    }

    private void OnDrawGizmosSelected()
    {
        //Draw attack and sight range
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, direction);

    }

}