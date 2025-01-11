using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AllyAI : MonoBehaviour
{
    private GameManager gameManager;
    private NavMeshAgent agent;

    public Transform player;
    public Animator animator;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    public float detectionRange = 20f;
    public float shootingRange = 10f;
    public float shootingInterval = 1f;
    private float runningInterval = 1f;
    public float moveSpeed = 3.5f;

    private float lastShotTime;
    private float lastStepSoundTime;
    private bool completedRotation = false;
    Vector3 targetDirection;

    // private bool isAlerted = false;
    private bool isRunning = false;
    // private bool isShooting = false; // isInShootingRange
    public bool isDead = false;

    public GameObject allySymbol;
    public GameObject allyDownSymbol;
    public GameObject allyMapMarker;

    public GameObject[] muzzleFlashes;
    public AudioSource shootingSound;
    public AudioSource runningSound;

    public float followDistance = 5f;  // Distance to keep from the player
    public float allySpacing = 2f;  // Minimum spacing from other allies

    private List<Transform> enemies;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        enemies = new List<Transform>();

    }

    void Update()
    {
        if (gameManager.CheckIfGamePaused())
        { return; }

        if (isDead)
        {
            agent.isStopped = true;
            animator.SetBool("isDead", isDead);

            allySymbol.SetActive(false);
            allyMapMarker.SetActive(false);

            allyDownSymbol.SetActive(true);
            
            return;
        }

        animator.SetBool("isRunning", isRunning);
        // animator.SetBool("isShooting", isShooting);

        FollowPlayer();

        DetectAndShootEnemies();
    }

    void FollowPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > followDistance)
        {
            agent.SetDestination(player.position);
            isRunning = agent.velocity.magnitude > 0.5f;
        }

        if (isRunning && (Time.time > lastStepSoundTime + runningInterval))
        {
            lastStepSoundTime = Time.time;
            runningSound.Play();
        }

        SeparateFromAllies();
    }

    void SeparateFromAllies()
    {
        Collider[] allies = Physics.OverlapSphere(transform.position, allySpacing);
        Vector3 separationVector = Vector3.zero;
        foreach (var ally in allies)
        {
            if (ally.gameObject != gameObject && ally.CompareTag("Ally"))
            {
                separationVector += transform.position - ally.transform.position;
            }
        }
        agent.destination += separationVector;
    }

    void DetectAndShootEnemies()
    {
        enemies.Clear();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                enemies.Add(hitCollider.transform);
            }
        }

        foreach (Transform enemy in enemies)
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null && !enemyAI.isDead)
            {
                targetDirection = enemy.transform.position - transform.position;
                targetDirection.y = 0;
                float dot = Vector3.Dot(transform.forward, targetDirection.normalized);

                completedRotation = dot >= 0.99f;
                if (Vector3.Distance(transform.position, enemy.position) <= shootingRange)
                {
                    RotateTowardsTarget(enemy.position);
                    if (Time.time > lastShotTime + shootingInterval && completedRotation)
                    {
                        Shoot(enemy.position);
                        // isShooting = true;
                    }
                }
            }
        }
    }

    void Shoot(Vector3 targetPosition)
    {
        lastShotTime = Time.time;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(targetPosition - bulletSpawnPoint.position));
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.SetDoNotHurtPlayer(true);
        }
        shootingSound.Play();

        if (muzzleFlashes != null && muzzleFlashes.Length > 0)
        {
            int index = Random.Range(0, muzzleFlashes.Length);
            GameObject flashInstance = Instantiate(muzzleFlashes[index], bulletSpawnPoint.position, Quaternion.identity);
            flashInstance.transform.forward = bulletSpawnPoint.forward;
            Destroy(flashInstance, 0.1f);
        }
    }

    void RotateTowardsTarget(Vector3 targetPosition)
    {
        targetDirection = targetPosition - transform.position;
        targetDirection.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10.0f); // Smooth rotation
    }
}

