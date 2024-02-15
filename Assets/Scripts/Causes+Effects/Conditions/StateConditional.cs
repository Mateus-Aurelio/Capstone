using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateConditional : ACondition
{
    private bool state;
    [SerializeField] private bool initState;

    public void SetCondition(bool given)
    {
        state = given;
    }

    public void SwapCondition()
    {
        state = !state;
    }

    public bool GetCondition()
    {
        return state;
    }

    public override bool ConditionMet()
    {
        return state;
    }
}
