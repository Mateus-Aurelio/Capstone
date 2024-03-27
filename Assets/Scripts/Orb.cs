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
        Debug.Log("ReleasedFromHand velocity magnitude: " + velocity.magnitude);
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
                    if (hand == null || hand.GetGripTime() < minGripTimeToGrab || !hand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Grip)) return;
                    else if (hand.GetGripTime() >= minGripTimeToPunch && hand.GetHandVelocity().magnitude > minVelocityToPunch)
                    {
                        PunchOrb(hand);
                    }
                    else
                    {
                        GrabOrb(hand, other.transform);
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
            if (hand == null || !hand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Grip) || hand.GetGripTime() < minGripTimeToGrab) return;
            else if (hand.GetGripTime() >= minGripTimeToGrab)
            {
                GrabOrb(hand, other.transform);
            }
        }
    }

    private void GrabOrb(PlayerHand hand, Transform parent)
    {
        SetHandObject(hand);
        transform.SetParent(parent);
        hand.TriggerHaptic(.05f, 0.1f);
        orbState = OrbState.held;
    }

    private void PunchOrb(PlayerHand hand)
    {
        Vector3 velocity = hand.GetHandVelocity() * 3;
        Debug.Log("Punch velocity magnitude: " + velocity.magnitude + "");
        hand.TriggerHaptic(Mathf.Lerp(0.1f, 1f, velocity.magnitude / 18f), 0.1f);
        ReleasedFromHand(velocity);
        orbState = OrbState.released;
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