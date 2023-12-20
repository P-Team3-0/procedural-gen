using UnityEngine;
using UnityEngine.AI;

public class flyingEnemy : enemy
{
    public float flyingHeight = 10f;
    private Vector3 targetPosition;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        // agent.updateUpAxis = false; // This allows for movement in 3D space
    }

    protected override void AttackPlayer()
    {
        Debug.Log("AttackPlayer");
    }
    protected override void Patroling()
    {
        if (!base.walkPointSet) SearchWalkPoint();

        if (base.walkPointSet)
            transform.LookAt(walkPoint);
        Debug.Log(walkPoint);
        Debug.Log(walkPointSet);
        transform.Rotate(0, 180, 0);
        agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            base.walkPointSet = false;
    }

    protected override void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 6f, whatIsGround))
            walkPointSet = true;

    }

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        Debug.Log(playerInAttackRange);
        Debug.Log(playerInSightRange);
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }
}