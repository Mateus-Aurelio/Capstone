using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellbook : MonoBehaviour
{
    [SerializeField] private SpellCircle spellCircle;
    [SerializeField] private GameObject spellBook;
    [SerializeField] private GameObject spellBookLeft;
    [SerializeField] private GameObject spellBookRight;
    [SerializeField] private Transform spellBookBook;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform leftPivot;
    [SerializeField] private Transform rightPivot;
    [SerializeField] private PlayerHand lefthand;
    [SerializeField] private float bookSpeed = 5;
    private bool castingMode = true;
    private bool waitForUninput = false;

    void Start()
    {
        SetCastingMode(true);
    }

    void Update()
    {
        float gripAmount = VRInput.ButtonPressedAmountInTenths(lefthand.GetInputHand(), UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Grip);
        UpdateBookPivots(gripAmount);

        // if (!waitForUninput && VRInput.ButtonPressed(UnityEngine.XR.XRNode.LeftHand, UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.MenuButton))
        if (!waitForUninput && gripAmount > 0.75f)
        {
            waitForUninput = true;
            SetCastingMode(!castingMode);
        }
        // else if (waitForUninput && !VRInput.ButtonPressed(UnityEngine.XR.XRNode.LeftHand, UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.MenuButton))
        else if (waitForUninput && gripAmount < 0.75f)
        {
            waitForUninput = false;
        }

        // if (!castingMode && VRInput.ButtonPressed(UnityEngine.XR.XRNode.LeftHand, UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
        if (!castingMode && gripAmount < 0.75f)
        {
            if (!spellBook.gameObject.activeInHierarchy)
            {
                spellBook.gameObject.SetActive(true);
                spellBookLeft.gameObject.SetActive(true);
                spellBookRight.gameObject.SetActive(true);
            }
            spellBook.transform.LookAt(spellBook.transform.position - (playerCamera.transform.position - spellBook.transform.position));
        }
        else
        {
            if (spellBook.gameObject.activeInHierarchy)
            {
                spellBook.gameObject.SetActive(false);
                spellBookLeft.gameObject.SetActive(false);
                spellBookRight.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateBookPivots(float gripAmount)
    {
        //leftPivot.localRotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(leftPivot.localEulerAngles.z, Mathf.Lerp(-2, -90, gripAmount), Time.deltaTime * bookSpeed));
        //rightPivot.localRotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(rightPivot.localEulerAngles.z, Mathf.Lerp(2, 90, gripAmount), Time.deltaTime * bookSpeed));
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
        spellBookLeft.gameObject.SetActive(!castingMode);
        spellBookRight.gameObject.SetActive(!castingMode);
    }
}
