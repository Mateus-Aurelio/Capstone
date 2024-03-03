using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private XRNode inputHand;
    [SerializeField] private PlayerHand otherHand;
    [SerializeField] private CharacterController characterController;

    private bool touchSpellMode = false;
    private bool moveEarthMode = false;
    private bool gripWasDown = false;
    [SerializeField] private GameObject touchSpellEffect;
    private Transform movingEarth;
    private Vector3 moveRelativePos;
    private Vector3 tempMove;

    void Start()
    {
        
    }

    void Update()
    {
        if (moveEarthMode)
        {
            if (VRInput.ButtonPressed(inputHand, InputHelpers.Button.Grip))
            {
                if (!gripWasDown) moveRelativePos = transform.position;
                tempMove = Vector3.ClampMagnitude((transform.position - moveRelativePos), 6.5f * Time.deltaTime);
                /*if (characterController.velocity.magnitude <= 0.01f)
                {
                    tempMove = Vector3.ClampMagnitude((transform.position - moveRelativePos)*3, 6.5f * Time.deltaTime);
                }*/
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
    }
}
