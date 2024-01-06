using System.Collections;
using System.Collections.Generic;
using Sketchfab;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    //Unity components/objects
    public GameObject player;

    public NavMeshAgent agent;

    public LayerMask whatIsGround, whatIsPlayer;

    //Movement variables
    public Vector3 walkPoint;
    protected bool walkPointSet;
    public float walkPointRange;

    //Attack variables
    public float timeBetweenAttacks;
    public float playerDistance;
    protected bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //Enemy variables
    public int health;
    public int attack;

    private Vector3 force;


    //Call every frame
    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange)
        {
            GetComponent<Animator>().SetTrigger("PlayerAway");
            ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            transform.LookAt(walkPoint);
        if (!gameObject.CompareTag("Goblin"))
            transform.Rotate(0, 180, 0);
        agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    protected virtual void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    protected virtual void ChasePlayer()
    {
        transform.LookAt(player.transform);
        if (!gameObject.CompareTag("Goblin"))
            transform.Rotate(0, 180, 0);
        agent.SetDestination(player.transform.position);
    }

    protected virtual void AttackPlayer()
    {
        //Make sure enemy doesn't move
        transform.LookAt(player.transform);
        if (!gameObject.CompareTag("Goblin"))
            transform.Rotate(0, 180, 0);
        agent.SetDestination(transform.position);
        if (!alreadyAttacked && agent.remainingDistance < playerDistance)
        {
            //Attack code here
            GetComponent<Animator>().SetTrigger("Attack");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    protected virtual void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<player>().TakeDamage(attack);
            // Reduce the impact of collision 
            // force = collision.relativeVelocity * 0.01f;
            // GetComponent<Rigidbody>().AddForce(-collision.relativeVelocity, ForceMode.Impulse);
            StopForce();
        }
    }

    private void StopForce()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        Debug.Log("Enemy health: " + health);
        if (health <= 0)
        {
            // Debug.Log("Enemy died");
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Draw attack and sight range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


}