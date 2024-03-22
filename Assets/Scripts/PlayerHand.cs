using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

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
    /*[SerializeField] private CharacterController characterController;

    private bool touchSpellMode = false;
    private bool moveEarthMode = false;
    private bool gripWasDown = false;
    [SerializeField] private GameObject touchSpellEffect;
    private Transform movingEarth;
    private Vector3 moveRelativePos;
    private Vector3 tempMove;*/
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
    }

    private void FixedUpdate()
    {
        if (movementChecker.Count > maxMovementChecks) movementChecker.RemoveAt(0);
        // if (currentOrb != null)
        movementChecker.Add(transform.position);

        /*Vector3 sum = new Vector3();
                foreach (Vector3 v in movementChecker)
                {
                    sum += v;
                }
                currentOrb.GetComponent<Rigidbody>().velocity = (sum / movementChecker.Count) / (Time.fixedDeltaTime * maxMovementChecks);*/
        //Vector3 startPos = movementChecker[movementChecker.Count - 1];
        //Vector3 endPos = movementChecker[0];
        //currentOrb.GetComponent<Orb>().ReleasedFromHand((startPos - endPos) / (Time.fixedDeltaTime * maxMovementChecks));
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
