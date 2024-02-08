using System.Collections;
using System.Collections.Generic;
// using Sketchfab;
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
    public int attack;
    private Transform room;
    private Vector3 roomSize;

    private Vector3 roomMin;
    private Vector3 roomMax;

    private bool isPlayerInRoom;

    public float deathDelay;

    public int health;

    //Call every frame
    private void Update()
    {
        health = GetComponent<LifeManager>().health;
        isPlayerInRoom = player.transform.position.x >= roomMin.x && player.transform.position.x <= roomMax.x &&
        player.transform.position.z >= roomMin.z && player.transform.position.z <= roomMax.z;
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (health > 0)
        {
            if ((!playerInSightRange && !playerInAttackRange) || (playerInSightRange && !isPlayerInRoom)) Patroling();
            if (playerInSightRange && !playerInAttackRange && isPlayerInRoom)
            {
                GetComponent<Animator>().SetTrigger("PlayerAway");
                ChasePlayer();
            }
            if (playerInSightRange && playerInAttackRange) AttackPlayer();
        }
        else
        {
            Death();
        }
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        room = transform.parent;
        roomSize = new Vector3(22, 0, 22);
        roomMin = room.position - roomSize / 2;
        roomMax = room.position + roomSize / 2;
        health = GetComponent<LifeManager>().health;
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
        if (walkPoint.x >= roomMin.x && walkPoint.x <= roomMax.x &&
        walkPoint.z >= roomMin.z && walkPoint.z <= roomMax.z && Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
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

    protected virtual void Death()
    {
        var animator = GetComponent<Animator>();
        animator.SetTrigger("Death");
        GetComponent<NavMeshAgent>().enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (health > 0)
            {
                player.GetComponent<LifeManager>().TakeDamage(attack);
            }
        }
        else
        {
            if (collision.gameObject.transform.parent.tag == "Hammer")
            {
                GetComponent<LifeManager>().TakeDamage(collision.gameObject.transform.parent.GetComponent<ProjectileMove>().damage);
            }

        }
        StopForce();
    }

    private void StopForce()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Spell") && gameObject.CompareTag("Goblin"))
        {
            ProjectileMove projectileMove = other.transform.parent.GetComponent<ProjectileMove>();
            int spellDamage = projectileMove.damage;
            this.GetComponent<LifeManager>().TakeDamage(spellDamage);
            Destroy(other.transform.parent.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Draw attack and sight range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, walkPoint);
    }


}