using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    private PlayerHand myHand;
    private OrbState orbState;

    [SerializeField] private float minGripTimeToGrab = 0;
    [SerializeField] private float maxGripTimeToGrab = 0.15f;
    [SerializeField] private float minGripTimeToPunch = 0.3f;
    [SerializeField] private float minVelocityToPunch = 0.5f;

    [SerializeField] private float minPunchVelocity = 10f;
    [SerializeField] private float maxPunchVelocity = 20f;
    [SerializeField] private float punchModifier = 4f;

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

    public void ReleasedFromHand(Vector3 velocity, bool useGravity=true)
    {
        Debug.Log("ReleasedFromHand velocity magnitude: " + velocity.magnitude);
        GetComponent<Rigidbody>().useGravity = useGravity;
        GetComponent<Rigidbody>().velocity = velocity * 1.1f;
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
        if (other.gameObject.CompareTag("PlayerHand") && orbState == OrbState.floating || orbState == OrbState.released)
        {
            PlayerHand hand = other.GetComponent<PlayerHand>();
            if (hand == null || !hand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Grip) || hand.GetGripTime() < minGripTimeToGrab) return;
            else if (hand.GetGripTime() >= minGripTimeToGrab && hand.GetGripTime() <= maxGripTimeToGrab)
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
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void PunchOrb(PlayerHand hand)
    {
        Vector3 velocity = hand.GetHandVelocity() * (Mathf.Clamp(hand.GetHandVelocity().magnitude * punchModifier, minPunchVelocity, maxPunchVelocity) / hand.GetHandVelocity().magnitude);
        Debug.Log("Punch velocity magnitude: " + velocity.magnitude + " from hand velocity magnitude " + hand.GetHandVelocity().magnitude);
        AttemptAutoAim(velocity);
        hand.TriggerHaptic(Mathf.Lerp(0.1f, 1f, velocity.magnitude / maxPunchVelocity), 0.1f);
        ReleasedFromHand(velocity, false);
        orbState = OrbState.released;
    }

    private void TouchedOtherHand()
    {

    }

    private void AttemptAutoAim(Vector3 velocity)
    {
        Debug.DrawRay(transform.position, velocity, Color.white);
        RaycastHit hit;
        foreach (Transform t in AutoAimPosList.GetList())
        {
            if (Physics.Raycast(transform.position, t.position - transform.position, out hit, Mathf.Infinity))
            {
                Debug.Log("angle: " + Vector3.Angle(velocity.normalized, (t.position - transform.position).normalized));
                Debug.DrawRay(transform.position, t.position - transform.position * hit.distance, Color.cyan);
                Debug.Log("Did Hit");
            }
            else
            {
                Debug.DrawRay(transform.position, t.position - transform.position * 50, Color.red);
                Debug.Log("Did not Hit");
            }
        }
    }
}

public enum OrbState
{
    floating = 0,
    held = 1,
    released = 2
}