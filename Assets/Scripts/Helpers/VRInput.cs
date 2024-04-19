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

    public static float ButtonPressedAmountInTenths(XRNode device, InputHelpers.Button button)
    {
        if (!ButtonPressed(device, button, 0.1f)) return 0.0f;
        else if (!ButtonPressed(device, button, 0.2f)) return 0.1f;
        else if (!ButtonPressed(device, button, 0.3f)) return 0.2f;
        else if (!ButtonPressed(device, button, 0.4f)) return 0.3f;
        else if (!ButtonPressed(device, button, 0.5f)) return 0.4f;
        else if (!ButtonPressed(device, button, 0.6f)) return 0.5f;
        else if (!ButtonPressed(device, button, 0.7f)) return 0.6f;
        else if (!ButtonPressed(device, button, 0.8f)) return 0.7f;
        else if (!ButtonPressed(device, button, 0.9f)) return 0.8f;
        else if (!ButtonPressed(device, button, 1.0f)) return 0.9f;
        return 1.0f;
    }

    public static void VRInputSubscribe()
    {

    }
}
