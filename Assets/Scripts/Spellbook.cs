using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellbook : MonoBehaviour
{
    [SerializeField] private SpellCircle spellCircle;
    /*[SerializeField] private Transform spellCircleParent;
    [SerializeField] private Vector3 spellCircleParentRelativeOffset;
    [SerializeField] private bool unparentSpellCircleOnEnterCastMode = false;*/
    [SerializeField] private GameObject spellBook;
    [SerializeField] private GameObject spellBookLeftPage;
    [SerializeField] private GameObject spellBookRightPage;
    [SerializeField] private GameObject spellBookCastingLeftPage;
    [SerializeField] private GameObject spellBookCastingRightPage;
    [SerializeField] private Transform spellBookBook;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform leftPivot;
    [SerializeField] private Transform rightPivot;
    [SerializeField] private PlayerHand lefthand;
    [SerializeField] private float bookSpeed = 5;
    [SerializeField] private float minGripAmount = 0.95f;
    // private bool castingMode = true;
    private bool waitForUninput = false;

    void Start()
    {
        // SetCastingMode(true);
    }

    void Update()
    {
        float gripAmount = VRInput.ButtonPressedAmountInTenths(lefthand.GetInputHand(), UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Grip);
        UpdateBookPivots(gripAmount);

        if (!waitForUninput && gripAmount > minGripAmount)
        {
            waitForUninput = true;
            // SetCastingMode(!castingMode);
        }
        else if (waitForUninput && gripAmount < minGripAmount)
        {
            waitForUninput = false;
        }

        /*if (!castingMode && gripAmount < minGripAmount)
        {
            if (!spellBook.gameObject.activeInHierarchy)
            {
                spellBook.gameObject.SetActive(true);
                spellBookLeftPage.gameObject.SetActive(true);
                spellBookRightPage.gameObject.SetActive(true);
                spellBookCastingLeftPage.gameObject.SetActive(false);
                spellBookCastingRightPage.gameObject.SetActive(false);
            }
            spellBook.transform.LookAt(spellBook.transform.position - (playerCamera.transform.position - spellBook.transform.position));
        }
        else
        {
            if (spellBook.gameObject.activeInHierarchy)
            {
                spellBook.gameObject.SetActive(false);
                spellBookLeftPage.gameObject.SetActive(false);
                spellBookRightPage.gameObject.SetActive(false);
                spellBookCastingLeftPage.gameObject.SetActive(true);
                spellBookCastingRightPage.gameObject.SetActive(true);
            }
        }*/
    }

    private void UpdateBookPivots(float gripAmount)
    {
        //leftPivot.localRotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(leftPivot.localEulerAngles.z, Mathf.Lerp(-2, -90, gripAmount), Time.deltaTime * bookSpeed));
        //rightPivot.localRotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(rightPivot.localEulerAngles.z, Mathf.Lerp(2, 90, gripAmount), Time.deltaTime * bookSpeed));
        leftPivot.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(-2, -90, gripAmount));
        rightPivot.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(2, 90, gripAmount));
        spellBookBook.localPosition = new Vector3(Mathf.Lerp(.06f, 0.01f, gripAmount), -0.021f, -0.035f);
    }

    /*public void SetCastingMode(bool given)
    {
        if (castingMode == given) return;
        castingMode = given;
        if (!castingMode)
        {
            spellCircle.ResetCasting();
        }
        spellCircle.gameObject.SetActive(castingMode);
        if (castingMode)
        {
            spellCircle.ResetCasting();
        }
        spellBook.SetActive(!castingMode);
        spellBookLeftPage.gameObject.SetActive(!castingMode);
        spellBookRightPage.gameObject.SetActive(!castingMode);
        spellBookCastingLeftPage.gameObject.SetActive(castingMode);
        spellBookCastingRightPage.gameObject.SetActive(castingMode);
    }*/
}
