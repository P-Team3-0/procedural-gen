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

    public GameObject bossProjectile;

    private GameObject firePoint;
    public Vector3 direction;





    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void Update()
    {
        firePoint = GameObject.Find("FirePoint");
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
            GameObject fireBreath = Instantiate(bossProjectile, firePoint.transform.position, Quaternion.identity);
            direction = (player.transform.position - firePoint.transform.position).normalized;
            float distance = Vector3.Distance(player.transform.position, firePoint.transform.position);

            projectileMovement bossProjectileScript = fireBreath.GetComponent<projectileMovement>();
            if (bossProjectileScript != null)
            {
                bossProjectileScript.SetDirection(direction);
                bossProjectileScript.SetSpeed(distance);

            }

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            alreadyAttacked = true;
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
