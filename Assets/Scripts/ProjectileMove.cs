using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{

    public float speed;
    public float fireRate;

    public int damage;


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
        Debug.Log("Spell Collision: " + other.name);
        if (LayerMask.LayerToName(other.layer) == "Enemy")
        {
            other.GetComponent<LifeManager>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }

}
