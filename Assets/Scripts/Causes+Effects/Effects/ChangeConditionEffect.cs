using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeConditionEffect : AEffect
{
    [SerializeField] SimpleCondition simpleCondition;

    public override void DoEffect()
    {
        simpleCondition.ChangeConditionState();
    }
}
