using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "newForceStatusEffect", menuName = "StatusEffect/Force", order = 3)]
public class ForceStatusEffect : StatusEffect
{
    [SerializeField] private Vector3 force;
    [SerializeField] private ForceMode forceMode;
    private Rigidbody rb;

    public override void ApplyStatusEffect(StatusEffectHandler statusEffectHandler)
    {
        duration = 999;
        NavMeshAgent agent = statusEffectHandler.GetNavMeshAgent();
        rb = statusEffectHandler.GetRigidbody();
        if (agent == null || rb == null) return;
        agent.enabled = false;
        rb.AddForce(force, forceMode);
    }

    public override void UpdateStatusEffect(StatusEffectHandler statusEffectHandler)
    {
        if (rb != null && rb.velocity.magnitude <= 0.1f)
        {
            NavMeshAgent agent = statusEffectHandler.GetNavMeshAgent();
            //rb = statusEffectHandler.GetRigidbody();
            //if (agent == null || rb == null) return;
            if (agent == null) return;
            agent.enabled = true;
        }
        return;
    }

    public override void RemoveStatusEffect(StatusEffectHandler statusEffectHandler)
    {
        NavMeshAgent agent = statusEffectHandler.GetNavMeshAgent();
        //rb = statusEffectHandler.GetRigidbody();
        //if (agent == null || rb == null) return;
        if (agent == null) return;
        agent.enabled = true;
        return;
    }
}
