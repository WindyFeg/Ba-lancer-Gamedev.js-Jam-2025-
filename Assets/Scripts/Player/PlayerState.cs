using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackSpeed = 1f;
    public LayerMask enemyLayer;
    public int maxHealth = 100;

    private int currentHealth;
    private float lastAttackTime = -Mathf.Infinity;
    private ModelSpine playerSpine;


    void Awake()
    {
        playerSpine = GetComponent<ModelSpine>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        TryAttack();
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TakeDamage(10);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Die();
        }
    }

    void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackSpeed)
        {
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
            if (hitEnemies.Length > 0)
            {
                Attack(hitEnemies);
                lastAttackTime = Time.time;
            }
        }
    }

    void Attack(Collider[] enemies)
    {
        foreach (Collider enemy in enemies)
        {
            Debug.Log("Attacking: " + enemy.name);
            enemy.GetComponent<ModelSpine>().hit_start();
        }
        playerSpine.attack_start();
    }


    public void TakeDamage(int damage)
    {

        currentHealth -= damage;
        Debug.Log("TakeDamage: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            playerSpine.hit_start();
        }
    }

    void Die()
    {
        playerSpine.death_start();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}