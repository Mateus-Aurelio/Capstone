using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateConditional : MonoBehaviour
{
    private bool state;
    [SerializeField] private bool initState;

    public void SetCondition(bool given)
    {
        state = given;
    }

    public bool GetCondition()
    {
        return state;
    }
}
