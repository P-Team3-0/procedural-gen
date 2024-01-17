using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireBallMovement : MonoBehaviour
{

    public float speed;
    public float fireRate;
    private Vector3 direction;


    private void Start()
    {

    }
    void Update()
    {
        if (speed != 0)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
        else
        {
            Debug.Log("No Speed");
        }

    }
    private void OnParticleCollision(GameObject other)
    {
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