using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[Serializable]
public enum Hand
{
    right = 0,
    left = 1,
    none = 2
}

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private Hand hand;
    private XRNode inputHand;
    [SerializeField] private PlayerHand otherHand;
    [SerializeField] private XRBaseController xrController;
    [SerializeField] private Transform relativeTransform;
    [SerializeField] private Vector3 grabSnapPos;
    private int handModel = 0;
    [SerializeField] private GameObject handOpen; // 0
    [SerializeField] private GameObject handOpening; // 1
    [SerializeField] private GameObject handClosing; // 2
    [SerializeField] private GameObject handFist; // 3
    [SerializeField] private GameObject handPoint; // 4
    [SerializeField] private GameObject handOpenFully; // 5
    private bool casting = false;
    private bool holdingBook = false;
    private float gripTime = 0;


    private List<Vector3> movementChecker = new List<Vector3>();
    [SerializeField] private int maxMovementChecks = 3;

    void Start()
    {
        if (hand == Hand.right)
        {
            inputHand = XRNode.RightHand;
        }
        else
        {
            inputHand = XRNode.LeftHand;
        }
        DisableAllHandModels();
        handOpen.SetActive(true);
        handModel = 0;
    }

    private void FixedUpdate()
    {
        if (movementChecker.Count > maxMovementChecks) movementChecker.RemoveAt(0);
        movementChecker.Add(transform.position);
    }

    private void Update()
    {
        if (VRInput.ButtonPressed(inputHand, InputHelpers.Button.Grip, 0.5f))
        {
            gripTime += Time.deltaTime;
        }
        else
        {
            gripTime = 0;
        }

        if (casting) return;
        float gripAmount = VRInput.ButtonPressedAmountInTenths(inputHand, InputHelpers.Button.Grip);
        if (holdingBook)
        {
            if (gripAmount > 0.85f)
            {
                if (handModel != 0)
                {
                    DisableAllHandModels();
                    handOpen.SetActive(true);
                    handModel = 0;
                }
            }
            else
            {
                if (handModel != 5)
                {
                    DisableAllHandModels();
                    handOpenFully.SetActive(true);
                    handModel = 5;
                }
            }
            return;
        }
        if (gripAmount > 0.85f)
        {
            if (handModel != 3)
            {
                DisableAllHandModels();
                handFist.SetActive(true);
                handModel = 3;
            }
        }
        else if (gripAmount > 0.55f)
        {
            if (handModel != 2)
            {
                DisableAllHandModels();
                handClosing.SetActive(true);
                handModel = 2;
            }
        }
        else if (gripAmount > 0.25f)
        {
            if (handModel != 1)
            {
                DisableAllHandModels();
                handOpening.SetActive(true);
                handModel = 1;
            }
        }
        else
        {
            if (handModel != 0)
            {
                DisableAllHandModels();
                handOpen.SetActive(true);
                handModel = 0;
            }
        }
    }

    private void DisableAllHandModels()
    {
        handOpen.SetActive(false);
        handOpening.SetActive(false);
        handClosing.SetActive(false);
        handFist.SetActive(false);
        handPoint.SetActive(false);
        handOpenFully.SetActive(false);
    }

    public void SetCasting(bool given)
    {
        casting = given;
        if (casting)
        {
            DisableAllHandModels();
            handPoint.SetActive(true);
            handModel = 4;
        }
    }

    public void SetHoldingBook(bool given)
    {
        holdingBook = given;
        if (holdingBook)
        {
            DisableAllHandModels();
            handOpenFully.SetActive(true);
            handModel = 5;
        }
    }

    public Hand GetHand()
    {
        return hand;
    }

    public Transform GetRelativeTransform()
    {
        return relativeTransform;
    }

    public PlayerHand GetOtherHand()
    {
        return otherHand;
    }

    public Vector3 GetHandPosition()
    {
        return transform.position - relativeTransform.position;
    }

    public void TriggerHaptic(float intensity, float duration)
    {
        if (intensity > 0 && duration > 0)
        {
            xrController.SendHapticImpulse(intensity, duration);
        }
    }

    public bool HandButtonPressed(InputHelpers.Button button, float pushThreshold = 0.5f)
    {
        return VRInput.ButtonPressed(inputHand, button, pushThreshold);
    }

    public XRNode GetInputHand()
    {
        return inputHand;
    }

    public float GetGripTime()
    {
        return gripTime;
    }

    public Vector3 GetHandVelocity()
    {
        Vector3 startPos = movementChecker[movementChecker.Count - 1];
        Vector3 endPos = movementChecker[0];
        return (startPos - endPos) / (Time.fixedDeltaTime * maxMovementChecks);
    }

    public Vector3 GetGrabSnapPos()
    {
        return grabSnapPos;
    }














    /* // old stuff for earth move spell
    void Update()
    {
        if (moveEarthMode)
        {
            if (VRInput.ButtonPressed(inputHand, InputHelpers.Button.Grip))
            {
                if (!gripWasDown) moveRelativePos = transform.position;
                tempMove = Vector3.ClampMagnitude((transform.position - moveRelativePos), 6.5f * Time.deltaTime);
                //if (characterController.velocity.magnitude <= 0.01f)
                //{
                //    tempMove = Vector3.ClampMagnitude((transform.position - moveRelativePos)*3, 6.5f * Time.deltaTime);
                //}
                movingEarth.Translate(tempMove, Space.World);
                moveRelativePos = transform.position;// Vector3.MoveTowards(moveRelativePos, transform.position, tempMove.magnitude);
            }
            if (gripWasDown && !VRInput.ButtonPressed(inputHand, InputHelpers.Button.Grip)) ExitMoveEarthMode();

            gripWasDown = VRInput.ButtonPressed(inputHand, InputHelpers.Button.Grip);
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (!touchSpellMode) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Earth"))
        {
            ExitTouchSpellMode();
            movingEarth = other.transform;
            EnterMoveEarthMode();
        }
    }

    public void EnterTouchSpellMode()
    {
        touchSpellMode = true;
        touchSpellEffect.SetActive(true);
    }

    public void ExitTouchSpellMode()
    {
        touchSpellMode = false;
        touchSpellEffect.SetActive(false);
    }

    public void EnterMoveEarthMode()
    {
        gripWasDown = false;
        moveEarthMode = true;
        movingEarth.GetComponent<ParticleSystem>().Play();
        movingEarth.GetComponent<Rigidbody>().useGravity = false;
        movingEarth.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        movingEarth.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void ExitMoveEarthMode()
    {
        moveEarthMode = false;
        movingEarth.GetComponent<ParticleSystem>().Stop();
        movingEarth.GetComponent<Rigidbody>().useGravity = true;
        movingEarth.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        movingEarth = null;
    }*/
}
