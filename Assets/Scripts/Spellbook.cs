using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spellbook : MonoBehaviour
{
    [SerializeField] private Transform leftPivot;
    [SerializeField] private Transform rightPivot;
    [SerializeField] private Transform turningPivot;
    [SerializeField] private PlayerHand lefthand;

    [SerializeField] private Image turningPageLeftImage;
    [SerializeField] private Image turningPageRightImage;
    [SerializeField] private Image leftPageImage;
    [SerializeField] private Image rightPageImage;
    private bool changingPages = false;
    [SerializeField] private float pageTurnSpeed = 1;

    [SerializeField] private float bookSpeed = 5;
    [SerializeField] private float minGripAmount = 0.95f;
    private bool waitForUninput = false;

    void Start()
    {

    }

    void Update()
    {
        if (changingPages) UpdateTurningPage();

        float gripAmount = VRInput.ButtonPressedAmountInTenths(lefthand.GetInputHand(), UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Grip);
        UpdateBookPivots(gripAmount);

        if (!waitForUninput && gripAmount > minGripAmount)
        {
            waitForUninput = true;
        }
        else if (waitForUninput && gripAmount < minGripAmount)
        {
            waitForUninput = false;
        }
    }

    private void UpdateBookPivots(float gripAmount)
    {
        //leftPivot.localRotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(leftPivot.localEulerAngles.z, Mathf.Lerp(-2, -90, gripAmount), Time.deltaTime * bookSpeed));
        //rightPivot.localRotation = Quaternion.Euler(0, 0, Mathf.SmoothStep(rightPivot.localEulerAngles.z, Mathf.Lerp(2, 90, gripAmount), Time.deltaTime * bookSpeed));
        leftPivot.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(-2, -90, gripAmount));
        rightPivot.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(2, 90, gripAmount));
        transform.localPosition = new Vector3(Mathf.Lerp(.06f, 0.01f, gripAmount), -0.021f, -0.035f);
    }

    private void UpdateTurningPage()
    {
        Debug.Log("UpdateTurningPage");
        turningPivot.localRotation = Quaternion.Euler(0, 0, turningPivot.localRotation.z + Time.deltaTime * pageTurnSpeed);
        Debug.Log(turningPivot.localRotation.z + Time.deltaTime * pageTurnSpeed);
        if (turningPivot.localRotation.eulerAngles.z >= 0 || turningPivot.localRotation.eulerAngles.z <= -180)
        {
            changingPages = false;
            Debug.Log("changingPages false now");
        }
    }

    public void TurnToTab(int tabID)
    {
        Debug.Log("TurnToTab");
        if (changingPages) return;
        Debug.Log("TurnToTab went");
        changingPages = true;
        pageTurnSpeed *= -1;
    }
}
