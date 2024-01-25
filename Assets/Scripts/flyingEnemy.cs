using UnityEngine;
using UnityEngine.AI;

public class flyingEnemy : enemy
{
    //Bullet variables
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float bulletSpawnDistance;

    private Transform room;
    private Vector3 roomSize;

    private Vector3 roomMin;
    private Vector3 roomMax;

    private bool isPlayerInRoom;


    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        room = transform.parent;
        roomSize = new Vector3(22, 0, 22);
        roomMin = room.position - roomSize / 2;
        roomMax = room.position + roomSize / 2;
    }
    private void Update()
    {
        isPlayerInRoom = player.transform.position.x >= roomMin.x && player.transform.position.x <= roomMax.x &&
        player.transform.position.z >= roomMin.z && player.transform.position.z <= roomMax.z;
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        if ((!playerInSightRange && !playerInAttackRange) || (playerInSightRange && !isPlayerInRoom)) Patroling();
        if (playerInSightRange && !playerInAttackRange && isPlayerInRoom)
        {
            ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
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
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void CreateBullet()
    {
        // Calculate the offset position
        // Vector3 offset = transform.forward * bulletSpawnDistance;
        // Vector3 spawnPosition = transform.position + offset;

        // Create a bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Calculate the direction of the bullet
        Vector3 direction = (player.transform.position - transform.position).normalized;

        // Set bullet speed
        bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
    }
    protected override void Patroling()
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

    protected override void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (walkPoint.x >= roomMin.x && walkPoint.x <= roomMax.x &&
        walkPoint.z >= roomMin.z && walkPoint.z <= roomMax.z && Physics.Raycast(walkPoint, -transform.up, 6f, whatIsGround))
            walkPointSet = true;

    }


    private void OnParticleCollision(GameObject other)
    {

        if (other.CompareTag("Spell"))
        {
            ProjectileMove projectileMove = other.transform.parent.GetComponent<ProjectileMove>();
            int spellDamage = projectileMove.damage;
            this.GetComponent<LifeManager>().TakeDamage(spellDamage);
            Destroy(other.transform.parent.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        StopForce();
    }

    private void StopForce()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (player.transform.position - transform.position).normalized);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, walkPoint);
    }
}