using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyAI : MonoBehaviour
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
    // public float moveSpeed = 3.5f;
    // public float gravity = -9.81f;

    private float shortestDistance;
    private GameObject closestTarget = null;

    private float lastShotTime;
    private float lastStepSoundTime;
    private bool completedRotation = false;
    private Vector3 targetDirection;
    // private Vector3 velocity;

    private bool isAlerted = false;
    private bool isRunning = false;
    private bool isShooting = false; // isInShootingRange
    public bool isDead = false;

    public GameObject enemySymbol;
    public GameObject enemyDownSymbol;
    public GameObject enemyMapMarker;

    public GameObject[] muzzleFlashes;
    public AudioSource shootingSound;
    public AudioSource runningSound;

    // private CharacterController controller;

    private List<AllyAI> allies;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        // controller = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        //find all game objecst with tag ally and add them to the list
        allies = new List<AllyAI>();

        GameObject[] allyObjects = GameObject.FindGameObjectsWithTag("Ally");
        foreach (GameObject allyObject in allyObjects)
        {
            allies.Add(allyObject.GetComponent<AllyAI>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.CheckIfGamePaused())
        { return; }

        if (isDead)
        {
            agent.isStopped = true;
            animator.SetBool("isDead", isDead);

            enemySymbol.SetActive(false);
            enemyMapMarker.SetActive(false);

            enemyDownSymbol.SetActive(true);

            return;
        }

        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isShooting", isShooting);

        UpdateClosestTarget();

        if (closestTarget != null)
        {
            Move();
        }

        if (isAlerted)
        {
            // We need this in case when Health script changed isAlerted to true
            if (shortestDistance > shootingRange)
            {
                isRunning = true;
                agent.SetDestination(closestTarget.transform.position);
            }

            if (Time.time > lastShotTime + shootingInterval)
            {
                Shoot();
            }

            targetDirection = closestTarget.transform.position - transform.position;
            targetDirection.y = 0;
            float dot = Vector3.Dot(transform.forward, targetDirection.normalized);
            completedRotation = dot >= 0.99f;
        }
    }

    void UpdateClosestTarget()
    {
        shortestDistance = Mathf.Infinity;
        closestTarget = null;

        CheckTargetDistance(player.gameObject, ref shortestDistance);

        foreach (AllyAI ally in allies)
        {
            if (ally != null && !ally.isDead)
            {
                CheckTargetDistance(ally.gameObject, ref shortestDistance);
            }
        }
    }

    void CheckTargetDistance(GameObject target, ref float shortestDistance)
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (distance < shortestDistance)
        {
            shortestDistance = distance;
            closestTarget = target;
        }
    }

    void Move()
    {
        if (shortestDistance < detectionRange)
        {
            isAlerted = true;
            agent.SetDestination(closestTarget.transform.position);
        }

        if (shortestDistance <= detectionRange && shortestDistance > shootingRange)
        {
            isRunning = true;
            isShooting = false;

            if (Time.time > lastStepSoundTime + runningInterval)
            {
                lastStepSoundTime = Time.time;
                runningSound.Play();
            }
        }
        if (shortestDistance <= shootingRange)
        {
            isRunning = false;
            isShooting = true;
            agent.isStopped = true;

            RotateTowardsTarget();
        }

        agent.isStopped = shortestDistance <= shootingRange;
    }

    void RotateTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10.0f); // Smooth rotation
    }

    void Shoot()
    {
        lastShotTime = Time.time;
        Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.LookRotation(closestTarget.transform.position - bulletSpawnPoint.position));

        shootingSound.Play();

        if (muzzleFlashes != null && muzzleFlashes.Length > 0 && completedRotation)
        {
            int index = Random.Range(0, muzzleFlashes.Length);
            GameObject flashInstance = Instantiate(muzzleFlashes[index], bulletSpawnPoint.position, Quaternion.identity);
            flashInstance.transform.forward = bulletSpawnPoint.forward;
            Destroy(flashInstance, 0.1f);
        }
    }

    // Health script is using this function
    public void EnemyIsAlerted()
    {
        Debug.Log("ovdje sam");
        isAlerted = true;
    }

    // This funtcion was used for movement before NavMesh was implemented
    /*
    void HandleMovement()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (isAlerted)
        {
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            if (distance > shootingRange)
            {
                Vector3 move = transform.forward * moveSpeed * Time.deltaTime;
                controller.Move(move);
            }
        }

        // Apply gravity
        if (!controller.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = 0;  // Reset gravity effect when grounded
        }
        controller.Move(velocity * Time.deltaTime);
    }
    */
}
