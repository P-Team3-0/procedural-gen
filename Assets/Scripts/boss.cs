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
            Vector3 targetPosition = player.transform.position;
            targetPosition.y = firePoint.transform.position.y; // Set the target y position to be the same as the firePoint's y position
            Debug.Log(targetPosition);
            direction = (targetPosition - firePoint.transform.position).normalized;
            Debug.Log(direction);

            projectileMovement bossProjectileScript = fireBreath.GetComponent<projectileMovement>();
            if (bossProjectileScript != null)
            {
                bossProjectileScript.SetDirection(direction);
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
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(firePoint.transform.position, player.transform.position);
    }

}
