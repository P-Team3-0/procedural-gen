using UnityEngine;
using UnityEngine.AI;

public class flyingEnemy : enemy
{
    //Bullet variables
    public GameObject bulletPrefab;
    public float bulletSpeed;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void AttackPlayer()
    {
        transform.LookAt(player.transform);
        transform.Rotate(0, 180, 0);
        agent.SetDestination(transform.position);
        if (!alreadyAttacked && agent.remainingDistance < playerDistance)
        {
            CreateBullet();
            alreadyAttacked = true;
            Invoke(nameof(base.ResetAttack), timeBetweenAttacks);
        }
    }

    private void CreateBullet()
    {
        // Create a bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Calculate the direction of the bullet
        Vector3 direction = (player.transform.position - transform.position).normalized;

        // Set bullet speed
        bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
    }
    protected override void Patroling()
    {
        if (!base.walkPointSet) SearchWalkPoint();

        if (base.walkPointSet)
            transform.LookAt(walkPoint);
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
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }
}