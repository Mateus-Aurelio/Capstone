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
    [SerializeField] private Transform handPos;
    [SerializeField] private Transform searchPivot;
    [SerializeField] private Transform cameraT;
    [SerializeField] private Transform playerFeet;
    [SerializeField] private float armLengthRatioToSearch = 0.75f;
    [SerializeField] private float armLengthRatioToPull = 0.1f;

    private SourceFinderState currentState = SourceFinderState.none;

    [SerializeField] private bool showSearchingDisplayObject = false;
    [SerializeField] private GameObject searchingDisplayObject;

    [SerializeField] private XRNode inputHand = XRNode.RightHand;

    [SerializeField] private OrbSpawner orbSpawner; 
    [SerializeField] private GameObject waterOrbPrefab;
    [SerializeField] private GameObject earthOrbPrefab;
    [SerializeField] private GameObject fireOrbPrefab;
    [SerializeField] private GameObject airOrbPrefab;

    private ElementSource tempSource = null;
    private ElementSource highlightedSource = null;

    [SerializeField] private MovementTracker handMovementTracker;

    private void Start()
    {

    }

    void Update()
    {
        searchPivot.position = BodyData.shouldersCenter + cameraT.position;
        searchPivot.LookAt(handPos.position, playerFeet.up);
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
        if (showSearchingDisplayObject && searchingDisplayObject != null) searchingDisplayObject.SetActive(false);
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
        if (showSearchingDisplayObject && searchingDisplayObject != null) searchingDisplayObject.SetActive(true);
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
    }

    private void NoneStateUpdate()
    {
        if (!VRInput.ButtonPressed(inputHand, InputHelpers.Button.Grip)
            && Vector3.Distance(handPos.position, BodyData.core + cameraT.position) > armLengthRatioToSearch * BodyData.armsLength)
        {
            EnterSearchingState();
        }
    }

    private void SearchingStateUpdate()
    {
        if (Vector3.Distance(handPos.position, BodyData.core + cameraT.position) <= armLengthRatioToSearch * BodyData.armsLength)
        {
            EnterNoneState();
            return;
        }

        if (!VRInput.ButtonPressed(inputHand, InputHelpers.Button.Grip))
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
        if (!VRInput.ButtonPressed(inputHand, InputHelpers.Button.Grip))
        {
            EnterNoneState();
            return;
        }
        if (VRInput.ButtonPressed(inputHand, InputHelpers.Button.Grip)
            && (Vector3.Distance(handPos.position, BodyData.shouldersCenter + cameraT.position) <= armLengthRatioToPull * BodyData.armsLength
                || (handPos.position.y < BodyData.shouldersCenter.y + cameraT.position.y 
                    && Vector3.Distance(new Vector3(handPos.position.x, (BodyData.shouldersCenter + cameraT.position).y, handPos.position.z), BodyData.shouldersCenter + cameraT.position) <= armLengthRatioToPull * BodyData.armsLength)))
        {
            EnterPulledState();
            return;
        }
    }

    private void PulledStateUpdate()
    {
        if (!VRInput.ButtonPressed(inputHand, InputHelpers.Button.Grip)) EnterNoneState();

        List<GameObject> points = handMovementTracker.GetPoints();
        Debug.Log("PulledStateUpdate, " + points.Count + " points");

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
