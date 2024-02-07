using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetConditionStateEffect : AEffect
{
    [SerializeField] SimpleCondition simpleCondition;
    [SerializeField] bool stateToSet = false;

    public override void DoEffect()
    {
        simpleCondition.SetConditionState(stateToSet);
    }
}