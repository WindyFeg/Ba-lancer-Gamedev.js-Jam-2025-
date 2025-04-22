using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerStats
{
    public float attackRange = 2f;
    public float attackSpeed = 1f;
    public LayerMask enemyLayer;
    public int maxHealth = 100;

    private int currentHealth;
    private float lastAttackTime = -Mathf.Infinity;
    private ModelSpine playerSpine;

    public static Player instance;
    void Awake()
    {
        instance = this;
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


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}