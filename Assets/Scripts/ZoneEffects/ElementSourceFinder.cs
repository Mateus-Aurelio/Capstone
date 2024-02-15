using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(CollisionStorer))]
public class ElementSourceFinder : ZoneEffect
{
    [SerializeField] private CollisionStorer collisionStorer;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private float maxHandDistance = 0.5f;
    private bool searching = false;
    [SerializeField] private GameObject searchObject;

    public override void DoEnterEffect(string zoneName)
    {

    }

    public override void DoExitEffect(string zoneName)
    {

    }

    private void Update()
    {
        if (!searching && ElementSearchGesture())
        {
            searching = true;
            searchObject.SetActive(searching);
        }
        else if (searching && !ElementSearchGesture())
        {
            searching = false;
            searchObject.SetActive(searching); 
        }
    }

    private bool ElementSearchGesture()
    {
        return VRInput.ButtonPressed(XRNode.LeftHand, InputHelpers.Button.PrimaryButton)
            && VRInput.ButtonPressed(XRNode.RightHand, InputHelpers.Button.PrimaryButton)
            && !VRInput.ButtonPressed(XRNode.LeftHand, InputHelpers.Button.Grip)
            && !VRInput.ButtonPressed(XRNode.RightHand, InputHelpers.Button.Grip)
            && !VRInput.ButtonPressed(XRNode.LeftHand, InputHelpers.Button.Trigger)
            && !VRInput.ButtonPressed(XRNode.RightHand, InputHelpers.Button.Trigger)
            && Vector3.Distance(leftHand.position, rightHand.position) < maxHandDistance;
    }
}
