using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeConditionEffect : AEffect
{
    [SerializeField] StateConditional simpleCondition;

    public override void DoEffect()
    {
        simpleCondition.SwapCondition();
    }
}
