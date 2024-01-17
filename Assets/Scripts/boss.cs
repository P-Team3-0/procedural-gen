using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class boss : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent agent;

    public LayerMask BossFlyingArea, whatIsPlayer;

    public float sightRange, attackRange, flameBreathRange;

    public bool playerInSightRange, playerInAttackRange, playerInFlameBreathRange;

    public float timeBetweenAttacks, timeBetweenFlameBreath;
    public float playerDistance;
    protected bool alreadyAttacked;

    private bool canMove = false;

    public GameObject fireBall;
    public GameObject flameBreath;

    private GameObject firePoint;
    public Vector3 direction;

    private bool hasFlameBreath = true;






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
        playerInFlameBreathRange = Physics.CheckSphere(transform.position, flameBreathRange, whatIsPlayer);
        if (playerInSightRange && !playerInAttackRange && canMove)
        {
            GetComponent<Animator>().SetTrigger("PlayerAway");
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
        if (agent.remainingDistance < playerDistance)
        {
            agent.GetComponent<Animator>().SetTrigger("Attack");

            // Invoke(nameof(ResetAttack), timeBetweenAttacks);
            alreadyAttacked = true;
        }
    }

    private void createProjectile()
    {
        if (hasFlameBreath && playerInFlameBreathRange)
        {
            GetComponent<Animator>().SetTrigger("Attack");
            GameObject flameBreathProjectile = Instantiate(flameBreath, firePoint.transform.position, Quaternion.identity);
            direction = (player.transform.position - firePoint.transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);
            flameBreathProjectile.transform.rotation = rotation;
            Invoke(nameof(ResetFlameBreath), timeBetweenFlameBreath);
            hasFlameBreath = false;
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Attack");
            GameObject fireBallProjectile = Instantiate(fireBall, firePoint.transform.position, Quaternion.identity);
            direction = (player.transform.position - firePoint.transform.position).normalized;
            float distance = Vector3.Distance(player.transform.position, firePoint.transform.position);
            fireBallMovement fireBallScript = fireBallProjectile.GetComponent<fireBallMovement>();
            if (fireBallScript != null)
            {
                fireBallScript.SetDirection(direction);
                fireBallScript.SetSpeed(distance);
            }
        }
    }
    protected virtual void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void ResetFlameBreath()
    {
        hasFlameBreath = true;
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
        Gizmos.DrawWireSphere(transform.position, flameBreathRange);
    }

}
