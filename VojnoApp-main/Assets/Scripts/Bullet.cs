using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private GameManager gameManager;
    private bool doNotHurtPlayer = false;

    private float speed = 100f;
    private float maxLifetime = 5f;
    private float damage = 10f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        Destroy(gameObject, maxLifetime);
    }

    public bool GetDoNotHurtPlayer()
    {
        return doNotHurtPlayer;
    }

    public void SetDoNotHurtPlayer(bool value)
    {
        doNotHurtPlayer = value;
    }

    void FixedUpdate()
    {
        if (!gameManager.CheckIfGamePaused())
        {
            rb.velocity = transform.forward * speed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!doNotHurtPlayer)
            {
                Health health = collision.gameObject.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }
        }
        else
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
