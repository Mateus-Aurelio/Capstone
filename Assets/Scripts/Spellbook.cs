using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spellbook : MonoBehaviour
{
    [SerializeField] private SpellCircle spellCircle;
    [SerializeField] private GameObject spellBook;
    [SerializeField] private GameObject spellBookLeftPage;
    [SerializeField] private GameObject spellBookRightPage;
    [SerializeField] private GameObject spellBookTurningPage;
    [SerializeField] private Transform spellBookBook;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform leftPivot;
    [SerializeField] private Transform rightPivot;
    [SerializeField] private Transform turningPivot;
    [SerializeField] private PlayerHand lefthand;

    [SerializeField] private Image turningPageLeftImage;
    [SerializeField] private Image turningPageRightImage;
    [SerializeField] private Image leftPageImage;
    [SerializeField] private Image rightPageImage;
    private bool changingPages = false;
    private float pageTurnSpeed = 1;

    [SerializeField] private float bookSpeed = 5;
    [SerializeField] private float minGripAmount = 0.95f;
    private bool waitForUninput = false;

    void Start()
    {

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

        if (changingPages) UpdateTurningPage();
    }

    private void UpdateBookPivots(float gripAmount)
    {
        //leftPivot.localRotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(leftPivot.localEulerAngles.z, Mathf.Lerp(-2, -90, gripAmount), Time.deltaTime * bookSpeed));
        //rightPivot.localRotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(rightPivot.localEulerAngles.z, Mathf.Lerp(2, 90, gripAmount), Time.deltaTime * bookSpeed));
        leftPivot.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(-2, -90, gripAmount));
        rightPivot.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(2, 90, gripAmount));
        spellBookBook.localPosition = new Vector3(Mathf.Lerp(.06f, 0.01f, gripAmount), -0.021f, -0.035f);
    }

    private void UpdateTurningPage()
    {
        if (pageTurnSpeed < 0) turningPivot.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(turningPivot.localRotation.z, -180, pageTurnSpeed * Time.deltaTime));
        else turningPivot.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(turningPivot.localRotation.z, 0, pageTurnSpeed * Time.deltaTime));
    }

    public void TurnToTab(int tabID)
    {

    }
}
