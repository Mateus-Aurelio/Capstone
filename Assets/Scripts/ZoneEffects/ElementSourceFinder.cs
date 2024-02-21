using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public enum SourceFinderState
{
    none = 0, 
    searching = 1, 
    pulling = 2, 
    pulled = 3
}

public class ElementSourceFinder : CollisionStorer
{
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform playerCenter;
    [SerializeField] private float handDistanceFromCenter = 0.5f;
    private SourceFinderState currentState = SourceFinderState.none;
    [SerializeField] private GameObject searchingDisplayObject;
    [SerializeField] private XRNode hand = XRNode.RightHand;

    [SerializeField] private OrbSpawner orbSpawner;
    [SerializeField] private GameObject waterOrbPrefab;
    [SerializeField] private GameObject earthOrbPrefab;
    [SerializeField] private GameObject fireOrbPrefab;
    [SerializeField] private GameObject airOrbPrefab;

    private ElementSource tempSource = null;
    private ElementSource highlightedSource = null;

    private void Start()
    {

    }

    void Update()
    {
        switch (currentState)
        {
            case SourceFinderState.none:
                NoneStateUpdate();
                break;
            case SourceFinderState.searching:
                SearchingStateUpdate();
                break;
            case SourceFinderState.pulling:
                PullingStateUpdate();
                break;
            case SourceFinderState.pulled:
                PulledStateUpdate();
                break;
            default:
                break;
        }
    }

    private void EnterNoneState()
    {
        currentState = SourceFinderState.none;
        this.ClearListAndDisableColliders();
        searchingDisplayObject.SetActive(false);
        if (highlightedSource != null)
        {
            highlightedSource.HideSource();
            highlightedSource.HidePulling();
        }
        highlightedSource = null;
    }

    private void EnterSearchingState()
    {
        currentState = SourceFinderState.searching;
        this.EnableColliders();
        searchingDisplayObject.SetActive(true);
    }

    private void EnterPullingState()
    {
        currentState = SourceFinderState.pulling;
        if (highlightedSource != null)
        {
            highlightedSource.HideSource();
            highlightedSource.ShowPulling();
        }
    }

    private void EnterPulledState()
    {
        currentState = SourceFinderState.pulled;
        if (highlightedSource != null)
        {
            highlightedSource.HidePulling();
        }
        if (highlightedSource == null)
        {
            EnterNoneState();
            return;
        }
        switch (highlightedSource.GetComponent<ElementSource>().GetElement())
        {
            case Element.none:
                Debug.Log("spawned null orb");
                orbSpawner.CreateOrb(null);
                break;
            case Element.earth:
                Debug.Log("spawned earth orb");
                orbSpawner.CreateOrb(earthOrbPrefab);
                break;
            case Element.water:
                Debug.Log("spawned water orb");
                orbSpawner.CreateOrb(waterOrbPrefab);
                break;
            case Element.fire:
                Debug.Log("spawned fire orb");
                orbSpawner.CreateOrb(fireOrbPrefab);
                break;
            case Element.air:
                Debug.Log("spawned air orb");
                orbSpawner.CreateOrb(airOrbPrefab);
                break;
            default:
                break;
        }
        EnterNoneState();
    }

    private void NoneStateUpdate()
    {
        if (!VRInput.ButtonPressed(hand, InputHelpers.Button.Grip)
            && Vector3.Distance(rightHand.position, playerCenter.position) > handDistanceFromCenter)
        {
            EnterSearchingState();
        }
    }

    private void SearchingStateUpdate()
    {
        if (Vector3.Distance(rightHand.position, playerCenter.position) <= handDistanceFromCenter)
        {
            EnterNoneState();
            return;
        }

        if (!VRInput.ButtonPressed(hand, InputHelpers.Button.Grip))
        {
            return;
        }

        if (GetObjectsColliding().Count > 0)
        {
            EnterPullingState();
        }
        else
        {
            EnterNoneState();
        }
    }

    private void PullingStateUpdate()
    {
        if (!VRInput.ButtonPressed(hand, InputHelpers.Button.Grip))
        {
            EnterNoneState();
            return;
        }
        if (Vector3.Distance(rightHand.position, playerCenter.position) <= handDistanceFromCenter
            && VRInput.ButtonPressed(hand, InputHelpers.Button.Grip))
        {
            EnterPulledState();
            return;
        }
    }

    private void PulledStateUpdate()
    {
        
    }

    protected override void ObjectsCollidingListChanged()
    {
        switch (currentState)
        {
            case SourceFinderState.none:
                return;

            case SourceFinderState.searching:
                List<GameObject> collidingObjects = GetObjectsColliding();
                if (collidingObjects.Count <= 0)
                {
                    if (highlightedSource != null) highlightedSource.HideSource();
                    highlightedSource = null;
                    return;
                }
                tempSource = collidingObjects[collidingObjects.Count - 1].GetComponent<ElementSource>();
                if (tempSource == null)
                {
                    if (highlightedSource != null) highlightedSource.HideSource();
                    highlightedSource = null;
                    return;
                }
                if (highlightedSource != null) highlightedSource.HideSource();
                highlightedSource = tempSource;
                highlightedSource.ShowSource();
                break;

            case SourceFinderState.pulling:
                return;

            case SourceFinderState.pulled:
                return;

            default:
                return;
        }
    }
}
