using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    private GameManager gameManager;

    public UIManager uiManager;
    public float maxHealth = 100f;
    private float currentHealth;
    private AllyAI allyAI;
    private EnemyAI enemyAI;

    public TextMeshProUGUI healthText;
    public GameObject hurtBackround;

    void Start()
    {
        currentHealth = maxHealth;
        if (gameObject.tag == "Player")
        {
            UpdateHealthDisplay();
        }
        else if (gameObject.tag == "Ally")
        {
            allyAI = GetComponent<AllyAI>();
        }
        else if (gameObject.tag == "Enemy")
        {
            enemyAI = GetComponent<EnemyAI>();
        }

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManager.CheckIfGamePaused())
        { return; }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        if (gameObject.tag == "Enemy" && currentHealth < maxHealth)
        {
            enemyAI.EnemyIsAlerted();
        }

        if (gameObject.tag == "Player")
        {
            UpdateHealthDisplay();
            StartCoroutine(HurtScreen());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthDisplay()
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}/{maxHealth}";
            if (currentHealth <= 30)
            {
                healthText.color = Color.red;
            }
            else
            {
                healthText.color = Color.white;
            }
        }
        else
        {
            Debug.Log("No text component assigned");
        }
    }

    private IEnumerator HurtScreen()
    {
        if (hurtBackround != null)
        {
            hurtBackround.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            hurtBackround.SetActive(false);
        }
    }

    private void Die()
    {
        if (gameObject.tag == "Player")
        {
            uiManager.TriggerLoseScreen();
        }
        else if (gameObject.tag == "Ally")
        {
            allyAI.isDead = true;
        }
        else
        {
            enemyAI.isDead = true;
            Destroy(gameObject, 20f);
        }
    }
}