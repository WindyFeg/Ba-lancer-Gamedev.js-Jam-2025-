public class PlayerStats : EntityStatsBase
{
    // Player specific stats can be added here

    #region Public Methods
    /// <summary>
    /// Handle the player's attack logic. 
    /// </summary>
    public override void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handle the death of the entity.
    /// This method should be overridden in derived classes to implement specific death behavior.
    /// </summary>
    public override void Die()
    {
        // Implement player death logic here
        Debug.Log($"{EntityName} has died.");
    }

    /// <summary>
    /// Damage the target entity.
    /// </summary>
    public override void Attack(EntityStatsBase target)
    {
        if (target == null)
        {
            Debug.LogWarning("Target is null. Cannot attack.");
            return false;
        }

        // Implement attack logic here
        target.TakeDamage(AttackDamage);
        Debug.Log($"{EntityName} attacked {target.EntityName} for {AttackDamage} damage.");
        return true;
    }

}