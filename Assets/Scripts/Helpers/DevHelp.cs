using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class DevHelp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsCounter;
    [SerializeField] private bool breakOnLeftMenu = false;

    void Update()
    {
        fpsCounter.text = "FPS: " + (int)(1 / Time.deltaTime);
        if (breakOnLeftMenu && VRInput.ButtonPressed(XRNode.LeftHand, InputHelpers.Button.MenuButton))
        {
            Debug.Break();
        }
    }
}
