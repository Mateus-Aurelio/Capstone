using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InverseConditional : ACondition
{
    [SerializeField] private ACondition conditionToInvert;

    public override bool ConditionMet()
    {
        return !conditionToInvert.ConditionMet();
    }
}
