using UnityEngine;
using UnityEngine.AI;

public class flyingEnemy : enemy
{
    //Bullet variables
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float bulletSpawnDistance;

    private Transform currentRoom;
    private Vector3 size;

    private Vector3 min;
    private Vector3 max;

    private bool playerInRoom;

    public GameObject destroyEffect;



    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        currentRoom = transform.parent;
        size = new Vector3(22, 0, 22);
        min = currentRoom.position - size / 2;
        max = currentRoom.position + size / 2;
        health = GetComponent<LifeManager>().health;
    }
    private void Update()
    {
        health = GetComponent<LifeManager>().health;
        if (health > 0)
        {
            playerInRoom = player.transform.position.x >= min.x && player.transform.position.x <= max.x &&
            player.transform.position.z >= min.z && player.transform.position.z <= max.z;
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            if ((!playerInSightRange && !playerInAttackRange) || (playerInSightRange && !playerInRoom)) Patroling();
            if (playerInSightRange && !playerInAttackRange && playerInRoom)
            {
                ChasePlayer();
            }
            if (playerInSightRange && playerInAttackRange) AttackPlayer();
        }
        else
        {
            enemyWalk.Stop();
            Death();
        }
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
        Quaternion rotation = Quaternion.LookRotation(direction);
        bullet.transform.rotation = rotation;

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
        if (walkPoint.x >= min.x && walkPoint.x <= max.x &&
        walkPoint.z >= min.z && walkPoint.z <= max.z && Physics.Raycast(walkPoint, -transform.up, 6f, whatIsGround))
            walkPointSet = true;

    }

    protected override void Death()
    {
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject, deathDelay);
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
        if (collision.gameObject.transform.parent != null)
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