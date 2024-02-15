using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class OrbSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fireOrb;
    [SerializeField] private GameObject airOrb;
    [SerializeField] private GameObject waterOrb;
    [SerializeField] private GameObject earthOrb;

    [SerializeField] private Vector3 spawnPos;
    [SerializeField] private Transform normalParent;
    [SerializeField] private Transform movingParent;
    [SerializeField] private Transform cameraTransform;
    private Transform currentParent;
    private bool moving = false;
    private GameObject currentOrb = null;
    private float CD = 0; // CHANGE TO A COROUTINE 

    void Start()
    {
        
    }

    void Update()
    {
        if (CD <= 0) OrbInputCheck();

        if (!moving && VRInput.ButtonPressed(XRNode.LeftHand, InputHelpers.Button.Grip))
        {
            moving = true;
            currentParent = movingParent;
            if (currentOrb != null) currentOrb.transform.parent = currentParent.transform;
        }
        else if (moving && !VRInput.ButtonPressed(XRNode.LeftHand, InputHelpers.Button.Grip))
        {
            moving = false;
            currentParent = normalParent;
            if (currentOrb != null) currentOrb.transform.parent = currentParent.transform;
        }
    }

    private void OrbInputCheck()
    {
        if (VRInput.ButtonPressed(XRNode.RightHand, InputHelpers.Button.PrimaryButton))
        {
            CreateOrb(earthOrb);
        }
        if (VRInput.ButtonPressed(XRNode.RightHand, InputHelpers.Button.SecondaryButton))
        {
            CreateOrb(airOrb);
        }
        if (VRInput.ButtonPressed(XRNode.LeftHand, InputHelpers.Button.PrimaryButton))
        {
            CreateOrb(waterOrb);
        }
        if (VRInput.ButtonPressed(XRNode.LeftHand, InputHelpers.Button.SecondaryButton))
        {
            CreateOrb(fireOrb);
        }
    }

    private void CreateOrb(GameObject prefab)
    {
        if (currentOrb != null) Destroy(currentOrb);
        currentOrb = Instantiate(prefab, cameraTransform.forward + transform.position, Quaternion.identity, currentParent);
        StartCoroutine("Cooldown");
    }

    private IEnumerable Cooldown()
    {
        CD = 1;
        yield return new WaitForSeconds(1);
        CD = 0;
    }
}
