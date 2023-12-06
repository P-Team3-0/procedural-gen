using System.Collections;
using System.Collections.Generic;
using Sketchfab;
using UnityEngine;
using UnityEngine.AI;

public class enemymovement : MonoBehaviour
{
    //Unity components/objects
    public GameObject player;

    public NavMeshAgent agent;

    public LayerMask whatIsGround, whatIsPlayer;

    //Movement variables
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attack variables
    public float timeBetweenAttacks;
    public float playerDistance;
    bool alredyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //Call every frame
    void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            transform.LookAt(walkPoint);
        transform.Rotate(0, 180, 0);
        agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        transform.LookAt(player.transform);
        transform.Rotate(0, 180, 0);
        agent.SetDestination(player.transform.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        transform.LookAt(player.transform);
        transform.Rotate(0, 180, 0);
        agent.SetDestination(transform.position);


        if (!alredyAttacked && agent.remainingDistance < playerDistance)
        {
            //Attack code here
            GetComponent<Animator>().SetTrigger("Attack");
            Debug.Log("Before Attack" + alredyAttacked);
            alredyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            Debug.Log(alredyAttacked);
        }
        GetComponent<Animator>().SetTrigger("Attack");

    }
    private void ResetAttack()
    {
        Debug.Log("Reset Attack");
        alredyAttacked = false;
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