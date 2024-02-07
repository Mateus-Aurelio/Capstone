using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionChangeEffect : AEffect
{
    [SerializeField] private StateConditional conditional;
    public override void DoEffect()
    {
        conditional.SetCondition(true);
    }
}