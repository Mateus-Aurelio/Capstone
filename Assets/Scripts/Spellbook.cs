using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellbook : MonoBehaviour
{
    [SerializeField] private SpellCircle spellCircle;
    [SerializeField] private GameObject spellBook;
    [SerializeField] private Camera playerCamera;
    private bool castingMode = true;
    private bool waitForUninput = false;

    void Start()
    {
        SetCastingMode(true);
    }

    void Update()
    {
        if (!waitForUninput && VRInput.ButtonPressed(UnityEngine.XR.XRNode.LeftHand, UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.MenuButton))
        {
            waitForUninput = true;
            SetCastingMode(!castingMode);
        }
        else if (waitForUninput && !VRInput.ButtonPressed(UnityEngine.XR.XRNode.LeftHand, UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.MenuButton))
        {
            waitForUninput = false;
        }

        if (!castingMode && VRInput.ButtonPressed(UnityEngine.XR.XRNode.LeftHand, UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
        {
            if (!spellBook.gameObject.activeInHierarchy) spellBook.gameObject.SetActive(true);
            spellBook.transform.LookAt(spellBook.transform.position - (playerCamera.transform.position - spellBook.transform.position));
        }
        else
        {
            if (spellBook.gameObject.activeInHierarchy) spellBook.gameObject.SetActive(false);
        }
    }

    public void SetCastingMode(bool given)
    {
        castingMode = given;
        if (!castingMode) spellCircle.ResetCasting();
        spellCircle.gameObject.SetActive(castingMode);
        if (castingMode) spellCircle.ResetCasting();
        spellBook.SetActive(!castingMode);
    }
}
