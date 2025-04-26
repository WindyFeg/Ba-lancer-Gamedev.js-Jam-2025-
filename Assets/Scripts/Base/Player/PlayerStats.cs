using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : EntityStatsBase
{
    // Player specific stats can be added here
    public List<StatConfig> linkedStats = new();
    
    public const float StatMin = 1f;
    public const float StatMax = 10f;

    private bool isSyncing = false;
    [SerializeField] private GameObject popupTextPrefab;

    public void ApplyStatChange(StatType stat, float newValue)
    {
        if (isSyncing) return; // prevent loop
     
        // Check if 1 of the linked stats is min/max => Prevent base stat from changing
        if (!CanChangeStat(stat, newValue)) return;
        
        isSyncing = true;
        float currentValue = GetStat(stat);
        float delta = newValue - currentValue;
        SetStat(stat, newValue);

        // Change all linked stats
        foreach (var link in linkedStats)
        {
            if (link.BaseStat == stat)
            {
                foreach (var targetStat in link.LinkedStat)
                {
                    float targetValue = GetStat(targetStat);
                    float newTarget = targetValue - delta * link.Ratio;
                    SetStat(targetStat, Mathf.Clamp(newTarget, StatMin, StatMax));
                }
                break;
            }
        }

        isSyncing = false;
    }

    public bool CanChangeStat(StatType stat, float newValue)
    {
        float currentValue = GetStat(stat);
        float delta = newValue - currentValue;

        if (Mathf.Approximately(delta, 0f)) return true;

        foreach (var link in linkedStats)
        {
            if (link.BaseStat != stat) continue;

            foreach (var targetStat in link.LinkedStat)
            {
                float targetCurrent = GetStat(targetStat);
                float targetNew = targetCurrent - delta * link.Ratio;

                // If increasing stat, check if target would fall below min
                if (delta > 0 && targetNew < StatMin)
                    return false;

                // If decreasing stat, check if target would exceed max
                if (delta < 0 && targetNew > StatMax)
                    return false;
            }
        }

        return true;
    }


    public float GetStat(StatType stat)
    {
        return stat switch
        {
            StatType.ATK => AttackDamage,
            StatType.HP => MaxHealth,
            StatType.ATKSPEED => AttackSpeed,
            StatType.DEF => Armor,
            StatType.SPEED => Speed,
            StatType.RANGE => Range,
            _ => 0f
        };
    }

    public void SetStat(StatType stat, float value)
    {
        switch (stat)
        {
            case StatType.ATK: AttackDamage = value; break;
            case StatType.HP: MaxHealth = value; break;
            case StatType.ATKSPEED: AttackSpeed = value; break;
            case StatType.DEF: Armor = value; break;
            case StatType.SPEED: Speed = value; break;
            case StatType.RANGE: Range = value; break;
        }
    }
    // More Enemy specific stats can be added here


    #region Public Methods
    /// <summary>
    /// Handle the enemy's attack logic. 
    /// </summary>
    public void TakeDamage(float damage) {
        GetComponent<ModelSpine>().hit_start();
        GameObject popupText;
          if (this.tag == "Player")
        {
            Debug.Log($"Player taking damage: {damage} - Reduce by {Armor} DEF = {damage - Armor}");
        }
        else if (this.tag == "Enemy")
        {
            Debug.Log($"Enemy taking damage: {damage} - Reduce by {Armor} DEF = {damage - Armor}");
        }
        if (damage < Armor)
        {
            popupText = Instantiate(popupTextPrefab, transform.position + new Vector3(0, 3f), transform.rotation);
            popupText.GetComponent<PopupText>().ShowText("Blocked");
            return;
        }
        CurrentHealth -= (damage - Armor);
        popupText = Instantiate(popupTextPrefab, transform.position + new Vector3(0, 3f), transform.rotation);
        popupText.GetComponent<PopupText>().ShowText($"{damage - Armor}");
        if (CurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            Debug.Log($"Current Health: {CurrentHealth}");
        }   
    }

    /// <summary>
    /// Handle the death of the entity. 
    /// This method should be overridden in derived classes to implement specific death behavior.
    /// </summary>
    public void Die() {
        ModelSpine spine = GetComponent<ModelSpine>();
        if (this.tag == "Player")
        {
            Debug.Log("Player is dead!");
            spine.death_start();
            StartCoroutine(DieCoroutine(spine.get_duration(spine.death_anim)));
        }
        else if (this.tag == "Enemy")
        {
            spine.death_start();
            Debug.Log("Enemy is dead!");
            StartCoroutine(DieCoroutine(spine.get_duration(spine.death_anim)));
        }
     }
     IEnumerator DieCoroutine(float delay)
     {
         yield return new WaitForSeconds(delay);
         if (this.tag == "Player")
         {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            

         }
         else if (this.tag == "Enemy")
         {
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            GameManager.Instance.CheckNextLevel();
            Destroy(gameObject);
         }
     }

    /// <summary>
    /// Damage the target entity. 
    /// </summary>
    public void Attack(EntityStatsBase target) { }
    #endregion

}