using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Damage
{
    [SerializeField] private float damageAmount;
    [SerializeField] private DamageType damageType;
    [SerializeField] private List<StatusEffect> effectsToInflict = new List<StatusEffect>();
    
    public void DealDamage(AHealth health)
    {
        health.Damage(damageAmount, damageType);
        StatusEffectHandler effectHandler = health.GetComponent<StatusEffectHandler>();
        if (effectHandler == null) return;
        foreach (StatusEffect e in effectsToInflict)
        {
            effectHandler.AddStatusEffect(e);
        }
    }

    public void ChangeDamage(float change)
    {
        damageAmount += change;
    }

    public void SetDamage(float newDamage)
    {
        damageAmount = newDamage;
    }
}
