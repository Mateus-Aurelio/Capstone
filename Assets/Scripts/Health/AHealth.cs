using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private List<AHealthTracker> healthTrackers = new List<AHealthTracker>();
    private float health;
    private float healthRatio = 1;

    private void Start()
    {
        health = maxHealth;
        healthRatio = 1.0f;
    }

    public void ChangeHealth(float change)
    {
        health = Mathf.Clamp(health + change, 0, maxHealth);
        healthRatio = (float)((float)health / (float)maxHealth);
        foreach (AHealthTracker tracker in healthTrackers) tracker.HealthChanged(this);
        if (health <= 0) Die();
    }

    public virtual void Damage(float damage, DamageType damageType = DamageType.none)
    {
        ChangeHealth(-damage);
    }

    public virtual void Heal(float healthGained)
    {
        ChangeHealth(healthGained);
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealthRatio()
    {
        return healthRatio;
    }

    public virtual void Die()
    {

    }
}
