using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public static class VRInput
{
    public static bool ButtonPressed(XRNode device, InputHelpers.Button button, float pressThreshold = 0.1f)
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(device), button, out bool isPressed, pressThreshold);
        return isPressed;
    }
}
