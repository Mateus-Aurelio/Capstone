using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACause : MonoBehaviour
{
    [SerializeField] protected List<AEffect> effects = new List<AEffect>();
    [SerializeField] protected List<ACondition> conditionsNeeded = new List<ACondition>();

    protected void CauseEffects()
    {
        foreach (ACondition condition in conditionsNeeded) if (! condition.ConditionMet()) return;
        foreach (AEffect effect in effects) effect.DoEffect();
    }
}
