using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "newStopStatusEffect", menuName = "StatusEffect/Stop", order = 1)]
public class StopStatusEffect : StatusEffect
{

    public override void ApplyStatusEffect(StatusEffectHandler statusEffectHandler)
    {
        NavMeshAgent agent = statusEffectHandler.GetNavMeshAgent();
        if (agent != null)
        {
            //agent.speed *= speedModifier;
        }
    }

    public override void UpdateStatusEffect(StatusEffectHandler statusEffectHandler)
    {

    }

    public override void RemoveStatusEffect(StatusEffectHandler statusEffectHandler)
    {
        NavMeshAgent agent = statusEffectHandler.GetNavMeshAgent();
        if (agent != null)
        {
            //agent.speed /= speedModifier;
        }
    }
}
