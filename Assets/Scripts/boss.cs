using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class boss : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent agent;

    public LayerMask BossFlyingArea, whatIsPlayer;

    public float sightRange, attackRange;

    public bool playerInSightRange, playerInAttackRange;

    public float timeBetweenAttacks;
    public float playerDistance;
    protected bool alreadyAttacked;

    private bool canMove = false;





    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (playerInSightRange && !playerInAttackRange)
        {
            if (canMove)
                ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
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
        if (!alreadyAttacked && agent.remainingDistance < playerDistance)
        {
            //Attack code here
            // GetComponent<Animator>().SetTrigger("Attack");
            // alreadyAttacked = true;
            Debug.Log("Boss Attack");
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    protected virtual void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void BossCanMove()
    {
        canMove = true;
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
