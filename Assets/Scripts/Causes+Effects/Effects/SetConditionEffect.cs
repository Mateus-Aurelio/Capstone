using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetConditionEffect : AEffect
{
    [SerializeField] private StateConditional conditional;
    [SerializeField] private bool conditionToSet;

    public override void DoEffect()
    {
        conditional.SetCondition(conditionToSet);
    }
}
