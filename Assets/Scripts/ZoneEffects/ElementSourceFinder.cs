using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ElementSourceFinder : MonoBehaviour
{
    [SerializeField] private CollisionStorer collisionStorer;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private float maxHandDistance = 0.5f;
    private bool searching = false;
    [SerializeField] private GameObject searchObject;
    [SerializeField] private XRNode hand = XRNode.RightHand;

    private ElementSource currentSource = null;

    private void Start()
    {
        SetSearching(false);
    }

    void Update()
    {
        if (!searching && ElementSearchGesture())
        {
            SetSearching(true);
        }
        else if (searching && !ElementSearchGesture())
        {
            SetSearching(false);
        }
        if (searching)
        {
            foreach (GameObject g in collisionStorer.GetObjectsColliding())
            {
                currentSource = g.GetComponent<ElementSource>();
                if (currentSource != null)
                {
                    currentSource.ShowSource();
                }
            }
        }
    }

    private void SetSearching(bool given)
    {
        searching = given;
        if (!given) collisionStorer.ClearListAndSetInactive();
        else searchObject.SetActive(true);
    }

    private bool ElementSearchGesture()
    {
        return VRInput.ButtonPressed(hand, InputHelpers.Button.SecondaryButton)
            && !VRInput.ButtonPressed(hand, InputHelpers.Button.Grip)
            && !VRInput.ButtonPressed(hand, InputHelpers.Button.Trigger);
    }
}
