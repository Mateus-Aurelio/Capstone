using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Elevator : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (VRInput.ButtonPressed(XRNode.LeftHand, InputHelpers.Button.Trigger))
        {
            transform.Translate(Vector3.up * Time.deltaTime);
        }
        else if (VRInput.ButtonPressed(XRNode.LeftHand, InputHelpers.Button.Grip))
        {
            transform.Translate(Vector3.down * Time.deltaTime);
        }
    }
}
