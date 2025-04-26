using Base;
using UnityEngine;

public class Player : MonoBehaviour
{
    public const float AttackDelayTime = 3f;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    
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

        }
        if (Input.GetKeyDown(KeyCode.F2))
        {       

        }
    }

    void TryAttack()
    {
        if (Time.time >= lastAttackTime + (AttackDelayTime / playerBehaviour.AttackSpeed))
        {
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, playerBehaviour.Range, enemyLayer);
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
            enemy.GetComponent<PlayerBehaviour>().TakeDamage(playerBehaviour.AttackDamage);
        }
        playerSpine.attack_start();
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerBehaviour.Range);
    }
}