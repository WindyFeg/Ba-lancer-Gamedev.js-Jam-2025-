using UnityEngine;

public abstract class EnemyStatsBase : EntityStatsBase
{
    // More Enemy specific stats can be added here

    #region Public Methods
    /// <summary>
    /// Handle the enemy's attack logic. 
    /// </summary>
    public abstract void TakeDamage(float damage);

    /// <summary>
    /// Handle the death of the entity. 
    /// This method should be overridden in derived classes to implement specific death behavior.
    /// </summary>
    public abstract void Die();

    /// <summary>
    /// Damage the target entity. 
    /// </summary>
    public abstract void Attack(EntityStatsBase target);

    #endregion
}