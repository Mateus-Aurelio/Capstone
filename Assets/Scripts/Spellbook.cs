using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spellbook : MonoBehaviour
{
    private bool spellbookHeld = true;
    [SerializeField] private Transform spellbookHolder;
    [SerializeField] private Transform playerFloor;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Vector3 heldLocalPos = new Vector3(.06f, -0.021f, -0.035f);
    [SerializeField] private Vector3 heldLocalRot = new Vector3(-15, 0, -90);
    [SerializeField] private float unheldYPosOffset = -0.3f;

    [SerializeField] private Transform leftPivot;
    [SerializeField] private Transform rightPivot;
    [SerializeField] private Transform turningPivot;
    [SerializeField] private PlayerHand lefthand;

    [SerializeField] private Image turningPageLeftImage;
    [SerializeField] private Image turningPageRightImage;
    [SerializeField] private Image leftPageImage;
    [SerializeField] private Image rightPageImage;
    [SerializeField] private List<Sprite> pageSprites = new List<Sprite>();
    [SerializeField] private List<GameObject> tabs = new List<GameObject>(); // left tabs, turning tabs, right tabs
    private int pageID = 0;
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
        spellbookHolder.position = new Vector3(spellbookHolder.position.x, playerCamera.position.y + unheldYPosOffset, spellbookHolder.position.z);

        float gripAmount = VRInput.ButtonPressedAmountInTenths(lefthand.GetInputHand(), UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Grip);
        if (!waitForUninput && gripAmount > minGripAmount)
        {
            // waitForUninput = true;
        }
        else if (waitForUninput && gripAmount < minGripAmount)
        {
            waitForUninput = false;
        }

        if (!spellbookHeld) return;

        if (changingPages) UpdateTurningPage();

        UpdateBookPivots(gripAmount);

        
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
        //turningPivot.localRotation = Quaternion.Euler(0, 0, turningPivot.localRotation.z + Time.deltaTime * pageTurnSpeed);
        turningPivot.Rotate(new Vector3(0, 0, Time.deltaTime * pageTurnSpeed));
        /*if (turningPivot.localRotation.eulerAngles.z > 0 || turningPivot.localRotation.eulerAngles.z < -180)
        {
            changingPages = false;
        }*/
    }

    public void TurnToTab(int tabID)
    {
        if (!spellbookHeld) return;
        if (changingPages) return;
        if (pageID == tabID) return;

        if (tabID < pageID && pageTurnSpeed > 0 || tabID > pageID && pageTurnSpeed < 0)
        {
            pageTurnSpeed *= -1;
        }
        if (tabID > pageID)
        {
            leftPageImage.sprite = pageSprites[pageID * 2];
            turningPageRightImage.sprite = pageSprites[pageID * 2 + 1];
            rightPageImage.sprite = pageSprites[tabID * 2 + 1];
            turningPageLeftImage.sprite = pageSprites[tabID * 2];
        }
        else
        {
            rightPageImage.sprite = pageSprites[pageID * 2 + 1];
            turningPageLeftImage.sprite = pageSprites[pageID * 2];
            leftPageImage.sprite = pageSprites[tabID * 2];
            turningPageRightImage.sprite = pageSprites[tabID * 2 + 1];
        }
        for (int i = 0; i < tabs.Count / 3; i++)
        {
            if (i > pageID && i < tabID || i < pageID && i > tabID || (pageTurnSpeed > 0 && i == pageID))
            {
                tabs[i * 3].SetActive(false);
                tabs[i * 3 + 1].SetActive(true);
                tabs[i * 3 + 2].SetActive(false);
            }
        }

        pageID = tabID;
        turningPivot.gameObject.SetActive(true);
        changingPages = true;
        if (pageTurnSpeed < 0) turningPivot.localRotation = Quaternion.Euler(0, 0, -2);
        else turningPivot.localRotation = Quaternion.Euler(0, 0, -178);
        StartCoroutine("StopTurning");
    }

    private IEnumerator StopTurning()
    {
        yield return new WaitForSeconds(Mathf.Abs(180 / pageTurnSpeed));
        changingPages = false;
        turningPivot.gameObject.SetActive(false);
        leftPageImage.sprite = pageSprites[pageID * 2];
        rightPageImage.sprite = pageSprites[pageID * 2+ 1];
        for (int i = 0; i < tabs.Count / 3; i++)
        {
            tabs[i * 3].SetActive(pageID >= i);
            tabs[i * 3 + 1].SetActive(false);
            tabs[i * 3 + 2].SetActive(pageID < i);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (waitForUninput) return;
        if (spellbookHeld)
        {
            if (other.transform == spellbookHolder && lefthand.GetGripTime() > 0.2f)
            {
                spellbookHeld = false; 
                leftPivot.localRotation = Quaternion.Euler(0, 0, -90);
                rightPivot.localRotation = Quaternion.Euler(0, 0, 90);
                transform.position = spellbookHolder.position;
                transform.SetParent(spellbookHolder);
                waitForUninput = true;
            }
            return;
        }
;
        if (!other.CompareTag("PlayerHand")) return;
        PlayerHand hand = other.GetComponent<PlayerHand>();
        if (hand == null || hand.GetGripTime() <= 0 || hand.GetHand() != Hand.left) return;

        waitForUninput = true;
        spellbookHeld = true;
        transform.SetParent(lefthand.transform);
        transform.localPosition = heldLocalPos;
        transform.localRotation = Quaternion.Euler(heldLocalRot);
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (!waitForUninput) return;
        if (spellbookHeld)
        {
            if (other.transform == spellbookHolder)
            {
                waitForUninput = false;
            }
            return;
        }
;
        if (!other.CompareTag("PlayerHand")) return;
        PlayerHand hand = other.GetComponent<PlayerHand>();
        if (hand == null || hand.GetGripTime() <= 0 || hand.GetHand() != Hand.left) return;

        waitForUninput = false;
    }*/
}
