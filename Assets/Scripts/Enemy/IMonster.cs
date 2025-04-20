using GameDefine;
using UnityEngine;

/// <summary>
/// Abstract class representing a monster in the game.
/// Data is retrieved from GameConfig and modified by constant multipliers.
/// </summary>
public abstract class IMonster : MonoBehaviour
{
    [Tooltip("Monster's range bullet")]
    [SerializeField] private GameObject prefab; // Range bullet prefab
    [Tooltip("Monster's death effect")]
    [SerializeField] private GameObject particlePrefab; // Particle prefab for death effect
    
    // Constants for multiplying monster stats
    public const float IDLE_MULTIPLIER = 2.0f;
    public const float TRIGGER_MULTIPLIER = 1.5f;
    public const float ATTACK_MULTIPLIER = 1.0f;
    
    // Base monster properties
    public string monsterName;
    public float idleRange;
    public float triggerRange;
    public float attackRange;
    public float triggerTime = 1f; // Default trigger time to avoid continuous triggering
    
    // Current state of the monster
    public MonsterState currentState = MonsterState.Idle;
    
    // Enum to represent monster states
    public enum MonsterState
    {
        Idle,
        Trigger,
        Attack,
        Dead
    }
    
    /// <summary>
    /// Initialize the monster with data from GameConfig
    /// </summary>
    /// <param name="monsterName">Name of the monster to look up in GameConfig</param>
    public virtual void Initialize(string monsterName)
    {
        this.monsterName = monsterName;
        
        // Get monster data from GameConfig
        var monsterData = GameConfig.Instance.FindMonster("Goblin");
        if (monsterData != null)
        {
            idleRange = monsterData.Range * IDLE_MULTIPLIER;
            triggerRange = monsterData.Range * TRIGGER_MULTIPLIER;
            attackRange = monsterData.Range * ATTACK_MULTIPLIER;
            triggerTime = 1f;
        }
        else
        {
            Debug.LogError($"Monster with name {monsterName} not found in GameConfig");
        }
    }
    
    // Abstract methods that must be implemented by derived classes
    public abstract void OnIdle();
    public abstract void OnTrigger();
    public abstract void OnAttack();
    public abstract void OnDead();
    
    /// <summary>
    /// Changes the monster's state and calls the appropriate handler
    /// </summary>
    /// <param name="newState">The new state to transition to</param>
    public virtual void ChangeState(MonsterState newState)
    {
        currentState = newState;
        
        switch (currentState)
        {
            case MonsterState.Idle:
                OnIdle();
                break;
            case MonsterState.Trigger:
                OnTrigger();
                break;
            case MonsterState.Attack:
                OnAttack();
                break;
            case MonsterState.Dead:
                OnDead();
                break;
        }
    }
    
    /// <summary>
    /// Check if target is within attack range
    /// </summary>
    /// <param name="target">The target to check</param>
    /// <returns>True if target is within attack range</returns>
    public virtual bool IsTargetInRange(Transform target)
    {
        if (target == null)
            return false;
            
        float distance = Vector3.Distance(transform.position, target.position);
        return distance <= attackRange;
    }
    
    /// <summary>
    /// Handles death of the monster
    /// </summary>
    public virtual void Die()
    {
        ChangeState(MonsterState.Dead);
        
        // Destroy the game object and create particle effect
        Destroy(gameObject);
        CreateDeathParticleEffect();
    }
    
    /// <summary>
    /// Creates particle effect on monster death
    /// </summary>
    public virtual void CreateDeathParticleEffect()
    {
        // Implementation will depend on the specific monster
        // This method can be overridden in derived classes
    }
    
    // Update is called once per frame
    public virtual void Update()
    {
        // State machine logic can be implemented here
        // or in a separate method that's called from Update
    }
}