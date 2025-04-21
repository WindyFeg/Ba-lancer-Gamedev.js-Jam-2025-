using UnityEngine;

public abstract class PlayerStats : EntityStatsBase
{
    // Player specific stats can be added here

    // More Enemy specific stats can be added here


    #region Public Methods
    /// <summary>
    /// Handle the enemy's attack logic. 
    /// </summary>
    public void TakeDamage(float damage) {
        GetComponent<ModelSpine>().hit_start();
        Debug.Log("Taking damage: " + damage);
     }

    /// <summary>
    /// Handle the death of the entity. 
    /// This method should be overridden in derived classes to implement specific death behavior.
    /// </summary>
    public void Die() { }

    /// <summary>
    /// Damage the target entity. 
    /// </summary>
    public void Attack(EntityStatsBase target) { }
    #endregion

}