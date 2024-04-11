using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "newSlowStatusEffect", menuName = "StatusEffect/Slow", order = 0)]
public class SlowStatusEffect : StatusEffect
{
    [SerializeField] private float speedModifier = 1;

    public override void ApplyStatusEffect(StatusEffectHandler statusEffectHandler)
    {
        NavMeshAgent agent = statusEffectHandler.GetNavMeshAgent();
        if (agent != null)
        {
            agent.speed *= speedModifier;
        }
    }

    public override void UpdateStatusEffect(StatusEffectHandler statusEffectHandler)
    {
        return;
    }

    public override void RemoveStatusEffect(StatusEffectHandler statusEffectHandler)
    {
        NavMeshAgent agent = statusEffectHandler.GetNavMeshAgent();
        if (agent != null)
        {
            agent.speed /= speedModifier;
        }
    }
}
