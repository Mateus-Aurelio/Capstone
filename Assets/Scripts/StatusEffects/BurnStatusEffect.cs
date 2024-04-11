using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newBurnStatusEffect", menuName = "StatusEffect/Burn", order = 1)]
public class BurnStatusEffect : StatusEffect
{
    [SerializeField] private Damage damage;
    [SerializeField] private float damageTriggersPerSecond = 2;
    private float timer = 0;


    public override void ApplyStatusEffect(StatusEffectHandler statusEffectHandler)
    {
        timer = 0;
    }

    public override void UpdateStatusEffect(StatusEffectHandler statusEffectHandler)
    {
        timer += Time.deltaTime;
        if (timer < 1 / damageTriggersPerSecond) return;
        timer = 0;
        AHealth healthScript = statusEffectHandler.GetHealth();
        if (healthScript != null)
        {
            damage.DealDamage(healthScript);
        }
    }

    public override void RemoveStatusEffect(StatusEffectHandler statusEffectHandler)
    {
        timer = 0;
    }
}
