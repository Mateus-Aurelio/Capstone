using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCondition : ACondition
{
    [SerializeField] private bool conditionState = false;

    public void ChangeConditionState()
    {
        SetConditionState(!conditionState);
    }

    public void SetConditionState(bool newState)
    {
        conditionState = newState;
    }

    public override bool ConditionMet()
    {
        return conditionState;
    }
}
