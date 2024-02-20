using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ElementSourceFinder : CollisionStorer
{
    // [SerializeField] private CollisionStorer collisionStorer;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [SerializeField] private Transform playerCenter;
    // [SerializeField] private float maxHandDistance = 0.5f;
    [SerializeField] private float handDistanceFromCenter = 0.5f;
    private bool searching = false;
    [SerializeField] private GameObject searchObject;
    [SerializeField] private GameObject foundObject;
    [SerializeField] private XRNode hand = XRNode.RightHand;

    [SerializeField] private OrbSpawner orbSpawner;
    [SerializeField] private GameObject waterOrbPrefab;
    [SerializeField] private GameObject earthOrbPrefab;
    [SerializeField] private GameObject fireOrbPrefab;
    [SerializeField] private GameObject airOrbPrefab;

    private ElementSource currentSource = null;

    private void Start()
    {
        SetSearching(false);
    }

    void Update()
    {
        if (ElementPullingGesture())
        {
            List<GameObject> collidedObjects = this.GetObjectsColliding();
            if (collidedObjects.Count > 0)
            {
                foundObject = collidedObjects[0];
            }
        }
        else if (ElementPulledGesture())
        {
            if (foundObject != null)
            {
                switch ((int) foundObject.GetComponent<ElementSource>().GetElement())
                {
                    case (int) Element.none:
                        Debug.Log("spawned null orb");
                        orbSpawner.CreateOrb(null);
                        break;
                    case (int)Element.earth:
                        Debug.Log("spawned earth orb");
                        orbSpawner.CreateOrb(earthOrbPrefab);
                        break;
                    case (int)Element.water:
                        Debug.Log("spawned water orb");
                        orbSpawner.CreateOrb(waterOrbPrefab);
                        break;
                    case (int)Element.fire:
                        Debug.Log("spawned fire orb");
                        orbSpawner.CreateOrb(fireOrbPrefab);
                        break;
                    case (int)Element.air:
                        Debug.Log("spawned air orb");
                        orbSpawner.CreateOrb(airOrbPrefab);
                        break;
                    default:
                        break;
                }
            }
            SetSearching(false);
        }
        else if (!searching && ElementSearchGesture())
        {
            SetSearching(true);
        }
        else if (searching && !ElementSearchGesture())
        {
            SetSearching(false);
        }
    }

    private void SetSearching(bool given)
    {
        searching = given;
        if (!given)
        {
            List<GameObject> collidedObjects = this.GetObjectsColliding();
            GameObject collidedObject;
            while (collidedObjects.Count > 0)
            {
                collidedObject = collidedObjects[0];
                this.TryToExit(collidedObject);
                this.NewObjectUncollided(collidedObject);
                collidedObjects = this.GetObjectsColliding();
            }
            /*foreach (GameObject g in this.GetObjectsColliding())
            {
                this.TryToExit(g);
                this.NewObjectUncollided(g);
            }*/
            this.ObjectsCollidingListChanged();
            this.ClearListAndDisableColliders();
            if (foundObject != null)
            {
                currentSource = foundObject.GetComponent<ElementSource>();
                if (currentSource != null) currentSource.HideSource();
            }
            foundObject = null;
        }
        else
        {
            this.EnableColliders();
        }
        
        searchObject.SetActive(given);
    }

    private bool ElementSearchGesture()
    {
        return /*VRInput.ButtonPressed(hand, InputHelpers.Button.SecondaryButton)
            && */(searching || !VRInput.ButtonPressed(hand, InputHelpers.Button.Grip))
            // && !VRInput.ButtonPressed(hand, InputHelpers.Button.Trigger)
            && Vector3.Distance(rightHand.position, playerCenter.position) > handDistanceFromCenter;
    }

    private bool ElementPullingGesture()
    {
        return /*VRInput.ButtonPressed(hand, InputHelpers.Button.SecondaryButton)
            && */VRInput.ButtonPressed(hand, InputHelpers.Button.Grip)
            // && !VRInput.ButtonPressed(hand, InputHelpers.Button.Trigger)
            && Vector3.Distance(rightHand.position, playerCenter.position) > handDistanceFromCenter;
    }

    private bool ElementPulledGesture()
    {
        return /*VRInput.ButtonPressed(hand, InputHelpers.Button.SecondaryButton)
            && */VRInput.ButtonPressed(hand, InputHelpers.Button.Grip)
            // && !VRInput.ButtonPressed(hand, InputHelpers.Button.Trigger)
            && Vector3.Distance(rightHand.position, playerCenter.position) <= handDistanceFromCenter;
    }

    protected override void ObjectsCollidingListChanged()
    {

    }

    protected override void NewObjectCollided(GameObject newObject)
    {
        currentSource = newObject.GetComponent<ElementSource>();
        if (currentSource != null) currentSource.ShowSource();
        /*if (newObject.TryGetComponent<ElementSource>(out currentSource))
        {
            currentSource.ShowSource();
        }*/
    }

    protected override void NewObjectUncollided(GameObject newObject)
    {
        currentSource = newObject.GetComponent<ElementSource>();
        if (currentSource != null && currentSource != foundObject) currentSource.HideSource();
        /*if (newObject.TryGetComponent<ElementSource>(out currentSource))
        {
            currentSource.HideSource();
        }*/
    }
}
