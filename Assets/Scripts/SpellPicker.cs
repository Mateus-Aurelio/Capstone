using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellPicker : MonoBehaviour
{
    [SerializeField] private GameObject mainParent;
    [SerializeField] private GameObject elementsCanvas;
    [SerializeField] private GameObject spellsCanvas;
    [SerializeField] private Transform footT;
    //[SerializeField] private float distanceForHandTouch;
    [SerializeField] private Transform mainHand;
    [SerializeField] private List<PickerSpot> pickerSpots = new List<PickerSpot>();
    
    private bool pickerOpen;
    private float fadingTimerElements;
    private float fadingTimerSpells;
    [SerializeField] private float fadeInOutTime = 0.33f;

    void Start()
    {
        
    }

    void Update()
    {
        if (VRInput.ButtonPressed(UnityEngine.XR.XRNode.RightHand, UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
        {
            PickerOpenUpdate();
        } 
        else if (pickerOpen)
        {
            PickerOpenUpdate();
            PickerClosedUpdate();
        }
    }

    private void PickerOpenUpdate()
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
                if (!VRInput.ButtonPressed(UnityEngine.XR.XRNode.RightHand, UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
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
    }
}
