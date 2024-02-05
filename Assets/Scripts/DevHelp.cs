using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class DevHelp : MonoBehaviour
{
    void Update()
    {
        if (VRInput.ButtonPressed(XRNode.LeftHand, InputHelpers.Button.MenuButton))
        {
            Debug.Break();
        }
    }
}
