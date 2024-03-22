using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    private PlayerHand myHand;
    private OrbState orbState;

    [SerializeField] private float minGripTimeToGrab = 0;
    [SerializeField] private float minGripTimeToPunch = 0.3f;
    [SerializeField] private float minVelocityToPunch = 0.5f;

    private List<Vector3> movementChecker = new List<Vector3>();
    [SerializeField] private int maxMovementChecks = 3;


    private void Start()
    {
        orbState = OrbState.floating;
    }

    private void Update()
    {
        if (orbState != OrbState.held) return;

        if (myHand.GetGripTime() <= 0)
        {
            orbState = OrbState.released; 
            Vector3 startPos = movementChecker[movementChecker.Count - 1];
            Vector3 endPos = movementChecker[0];
            ReleasedFromHand((startPos - endPos) / (Time.fixedDeltaTime * maxMovementChecks));
        }
    }

    private void FixedUpdate()
    {
        if (orbState != OrbState.held) return;
        if (movementChecker.Count > maxMovementChecks) movementChecker.RemoveAt(0);
        movementChecker.Add(transform.position);
    }

    public void SetHandObject(PlayerHand givenHand)
    {
        myHand = givenHand;
    }

    public void ReleasedFromHand(Vector3 velocity)
    {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().velocity = velocity;
        transform.parent = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHand"))
        {
            switch (orbState)
            {
                case OrbState.floating:
                    PlayerHand hand = other.GetComponent<PlayerHand>();
                    if (hand == null || hand.GetGripTime() <= minGripTimeToGrab) return;
                    else if (hand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Grip) && hand.GetGripTime() >= minGripTimeToPunch && hand.GetHandVelocity().magnitude > minVelocityToPunch)
                    {
                        orbState = OrbState.released;
                        ReleasedFromHand(hand.GetHandVelocity());
                    }
                    else
                    {
                        SetHandObject(hand);
                        transform.SetParent(other.transform);
                        orbState = OrbState.held;
                    }
                    break;
                case OrbState.held:
                    if (other.gameObject != myHand)
                    {
                        TouchedOtherHand();
                    }
                    break;
                case OrbState.released:
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHand") && orbState == OrbState.floating)
        {
            PlayerHand hand = other.GetComponent<PlayerHand>();
            if (hand == null || hand.GetGripTime() <= minGripTimeToGrab) return;
            else if (hand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Grip) && hand.GetGripTime() > minGripTimeToPunch)
            {
                SetHandObject(hand);
                transform.SetParent(other.transform);
                orbState = OrbState.held;
            }
        }
    }

    private void TouchedOtherHand()
    {

    }

}

public enum OrbState
{
    floating = 0,
    held = 1,
    released = 2
}