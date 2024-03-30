using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPicker : MonoBehaviour
{
    //[SerializeField] private GameObject mainParent;
    [SerializeField] private GameObject elementsCanvas;
    [SerializeField] private List<PickerSpot> elementPickerSpots = new List<PickerSpot>();
    private List<PickerSpot> spellPickerSpots = new List<PickerSpot>();
    [SerializeField] private Transform footT;
    [SerializeField] private Transform cameraT;
    //[SerializeField] private float distanceForHandTouch;
    [SerializeField] private Transform mainHandT;
    [SerializeField] private PlayerHand mainHand;

    [SerializeField] private float maxPosToStay = 0.5f;
    private bool waitToUngrip = false;
    [SerializeField] private bool hideElementsOnceSelected = true;
    [SerializeField] private bool adjustPositionOnceElementSelected = true;
    [SerializeField] private bool includeElementsInSpellsState = false;

    private PickerState pickState;
    //private float fadingTimerElements;
    //private float fadingTimerSpells;
    //[SerializeField] private float fadeInOutTime = 0.33f;

    void Start()
    {
        EnterNoneState();
        foreach (PickerSpot elementSpot in elementPickerSpots)
        {
            elementSpot.HideSubSpots();
        }
    }

    void Update()
    {
        switch (pickState)
        {
            case PickerState.noneShown:
                NoneShownUpdate();
                break;
            case PickerState.elementsShown:
                ElementsShownUpdate();
                break;
            case PickerState.spellsShown:
                SpellsShownUpdate();
                break;
            default:
                break;
        }

        if (pickState != PickerState.noneShown && Vector3.Distance(transform.position, mainHandT.position) > maxPosToStay)
        {
            waitToUngrip = true;
            EnterNoneState();
        }

        /*if (VRInput.ButtonPressed(UnityEngine.XR.XRNode., UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
        {
            PickerOpenUpdate();
        } 
        else if (pickerOpen)
        {
            PickerOpenUpdate();
            PickerClosedUpdate();
        }*/
    }

    private void NoneShownUpdate()
    {
        if (waitToUngrip)
        {
            waitToUngrip = mainHand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger);
            return;
        }
        if (mainHand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
        {
            EnterElementsState();
        }
    }

    private void ElementsShownUpdate()
    {
        if (!mainHand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
        {
            EnterNoneState();
        }
        foreach (PickerSpot elementSpot in elementPickerSpots)
        {
            if (elementSpot.GetComponent<Collider>().bounds.Contains(mainHandT.transform.position))
            {
                elementSpot.RevealSubSpots();
                foreach (PickerSpot otherElementSpot in elementPickerSpots)
                {
                    if (otherElementSpot != elementSpot) otherElementSpot.HideSubSpots();
                }
                spellPickerSpots = elementSpot.GetSubSpots();
                EnterSpellsState();
                break;
            }
        }
    }

    private void SpellsShownUpdate()
    {
        foreach (PickerSpot spellSpot in spellPickerSpots)
        {
            if (spellSpot.GetComponent<Collider>().bounds.Contains(mainHandT.transform.position))
            {
                spellSpot.HandTouched(mainHand, this);
                if (!mainHand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
                {
                    spellSpot.HandReleased(mainHand, this);
                    EnterNoneState();
                    return;
                }
            }
            else
            {
                spellSpot.HandNotTouched(this);
            }
        }
        if (includeElementsInSpellsState)
        {
            foreach (PickerSpot elementSpot in elementPickerSpots)
            {
                if (elementSpot.GetComponent<Collider>().bounds.Contains(mainHandT.transform.position))
                {
                    elementSpot.RevealSubSpots();
                    elementSpot.HandTouched(mainHand, this);
                    foreach (PickerSpot otherElementSpot in elementPickerSpots)
                    {
                        if (otherElementSpot != elementSpot) otherElementSpot.HideSubSpots();
                    }
                    if (!mainHand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
                    {
                        elementSpot.HandReleased(mainHand, this);
                        EnterNoneState();
                        return;
                    }
                    spellPickerSpots = elementSpot.GetSubSpots();
                    EnterSpellsState();
                    break;
                }
                else
                {
                    elementSpot.HandNotTouched(this);
                }
            }
        }
        if (!mainHand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
        {
            EnterNoneState();
            return;
        }
    }

    private void EnterNoneState()
    {
        pickState = PickerState.noneShown;
        foreach (PickerSpot elementSpot in elementPickerSpots)
        {
            elementSpot.HideSpot();
        }
        elementsCanvas.SetActive(false);
        if (spellPickerSpots == null)
        {
            return;
        }
        foreach (PickerSpot spellSpot in spellPickerSpots)
        {
            spellSpot.HideSpot();
        }
        spellPickerSpots = null;
    }

    private void EnterElementsState()
    {
        pickState = PickerState.elementsShown;
        transform.position = mainHandT.position;
        transform.LookAt(cameraT);
        elementsCanvas.SetActive(true);
        foreach (PickerSpot elementSpot in elementPickerSpots)
        {
            elementSpot.RevealSpot();
        }
        if (spellPickerSpots == null)
        {
            return;
        }
        foreach (PickerSpot spellSpot in spellPickerSpots)
        {
            spellSpot.HideSpot();
        }
    }

    private void EnterSpellsState()
    {
        pickState = PickerState.spellsShown;
        if (adjustPositionOnceElementSelected)
        {
            transform.position = mainHandT.position;
            transform.LookAt(cameraT);
        }
        if (hideElementsOnceSelected)
        {
            foreach (PickerSpot elementSpot in elementPickerSpots)
            {
                elementSpot.HideSpot();
            }
        }
        foreach (PickerSpot spellSpot in spellPickerSpots)
        {
            spellSpot.RevealSpot();
        }
    }

    /*private void PickerOpenUpdate()
    {
        pickerOpen = true;
        foreach (PickerSpot spot in pickerSpots)
        {
            if (!spot.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (spot.GetComponent<Collider>().bounds.Contains(mainHand.transform.position))
            {
                spot.HandTouched();
                if (!VRInput.ButtonPressed(UnityEngine.XR.XRNode., UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
                {
                    spot.HandReleased();
                }
            } 
            else
            {
                spot.HandNotTouched();
            }
        }
    }

    private void PickerClosedUpdate()
    {
        pickerOpen = false;
    }*/
}

public enum PickerState
{
    noneShown = 0,
    elementsShown = 1,
    spellsShown = 2
}