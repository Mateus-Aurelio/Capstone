using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellbook : MonoBehaviour
{
    [SerializeField] private SpellCircle spellCircle;
    [SerializeField] private GameObject spellBook;
    [SerializeField] private Transform spellBookBook;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform leftPivot;
    [SerializeField] private Transform rightPivot;
    [SerializeField] private PlayerHand lefthand;
    private bool castingMode = true;
    private bool waitForUninput = false;

    void Start()
    {
        SetCastingMode(true);
    }

    void Update()
    {
        UpdateBookPivots();

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

    private void UpdateBookPivots()
    {
        float gripAmount = VRInput.ButtonPressedAmountInTenths(lefthand.GetInputHand(), UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Grip);
        leftPivot.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(-2, -90, gripAmount));
        rightPivot.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(2, 90, gripAmount));
        spellBookBook.localPosition = new Vector3(Mathf.Lerp(.06f, 0.01f, gripAmount), -0.021f, -0.035f);
    }

    public void SetCastingMode(bool given)
    {
        if (castingMode == given) return;
        castingMode = given;
        if (!castingMode) spellCircle.ResetCasting();
        spellCircle.gameObject.SetActive(castingMode);
        if (castingMode) spellCircle.ResetCasting();
        spellBook.SetActive(!castingMode);
    }
}
