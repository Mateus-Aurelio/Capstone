using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCause : ACause
{
    private enum InputType { GetKey, GetKeyDown, GetKeyUp };
    [SerializeField] private KeyCode inputKeyCode;
    [SerializeField] private InputType inputType = InputType.GetKey;

    void Update()
    {
        if (inputType == InputType.GetKey && Input.GetKey(inputKeyCode))
        {
            CauseEffects();
        }
        if (inputType == InputType.GetKeyDown && Input.GetKeyDown(inputKeyCode))
        {
            CauseEffects();
        }
        if (inputType == InputType.GetKeyUp && Input.GetKeyUp(inputKeyCode))
        {
            CauseEffects();
        }
    }
}
