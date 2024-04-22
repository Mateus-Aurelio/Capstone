using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCircle : MonoBehaviour
{
    private SpellCircleLocation lastLocation = SpellCircleLocation.none;
    private List<SpellCircleEdge> edges = new List<SpellCircleEdge>();
    [SerializeField] private GameObject visualsGameObject;
    [SerializeField] private Image circleImage;
    [SerializeField] private LineRenderer line;
    private Vector3[] linePositions = new Vector3[0];
    [SerializeField] private Transform playerCamera;
    [SerializeField] private List<GameObject> spells = new List<GameObject>();
    private List<SpellCirclePoint> spellCirclePoints = new List<SpellCirclePoint>();
    /*[SerializeField] private List<PlayerHand> hands = new List<PlayerHand>();
    private PlayerHand preparingHand = null;*/
    private PlayerHand castingHand = null;
    [SerializeField] private PlayerHand leftHand = null;
    [SerializeField] private PlayerHand rightHand = null;
    private bool ignoreUntilUninput = false;
    private Element element = Element.earth;

    [SerializeField] private float resourceGainSpeed = 1;
    [SerializeField] private ElementResource earthResource;
    [SerializeField] private ElementResource waterResource;
    [SerializeField] private ElementResource airResource;
    [SerializeField] private ElementResource fireResource;
    private float earthAmount = 8;
    private float waterAmount = 8;
    private float airAmount = 8;
    private float fireAmount = 8;

    private float lastTouchedTimer = 0f;
    private float lastTouchedTimeSet = 4;
    private float lastTouchFadeTime = 2;
    private float circleImageMinAlpha = 0.25f;

    private void Awake()
    {
        SetElement(element);
        ResetCasting();
    }

    private void Update()
    {
        if (lastTouchedTimer > 0)
        {
            lastTouchedTimer -= Time.deltaTime;
            if (lastTouchedTimer <= 0)
                circleImage.color = ColorHelpers.SetColorAlpha(circleImage.color, circleImageMinAlpha);
            else if (lastTouchedTimer < lastTouchFadeTime)
                circleImage.color = ColorHelpers.SetColorAlpha(circleImage.color, circleImageMinAlpha + lastTouchedTimer);
        }
        
        UpdateResources();
        if (lastLocation == SpellCircleLocation.none)
        {
            if (VRInput.ButtonPressed(UnityEngine.XR.XRNode.LeftHand, UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.PrimaryButton)) SetElement(Element.earth);
            else if (VRInput.ButtonPressed(UnityEngine.XR.XRNode.LeftHand, UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.SecondaryButton)) SetElement(Element.air);
            else if (VRInput.ButtonPressed(UnityEngine.XR.XRNode.RightHand, UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.PrimaryButton)) SetElement(Element.water);
            else if (VRInput.ButtonPressed(UnityEngine.XR.XRNode.RightHand, UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.SecondaryButton)) SetElement(Element.fire);
        }

        /* if (ignoreUntilUninput != null && !ignoreUntilUninput.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger)) ignoreUntilUninput = null;

        if (preparingHand == null)
        {
            foreach (PlayerHand hand in hands)
            {
                if (hand.GetHand() != Hand.right && hand != ignoreUntilUninput && hand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
                {
                    ResetCasting();
                    visualsGameObject.SetActive(true);
                    preparingHand = hand;
                    transform.position = preparingHand.transform.position;
                }
            }
        } 
        else if (!preparingHand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
        {
            ResetCasting();
            return;
        }*/

        if (!ignoreUntilUninput && rightHand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
        {
            ResetCasting();
            visualsGameObject.SetActive(true);
            ignoreUntilUninput = true;
            // transform.position = leftHand.transform.position;
        }

        visualsGameObject.transform.LookAt(visualsGameObject.transform.position - (playerCamera.position - visualsGameObject.transform.position));

        castingHand = rightHand;
        // if (castingHand == null) return; 

        if (!castingHand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
        {
            ignoreUntilUninput = false;
            CastAttempt();
        }
    }

    public bool SpellCirclePointTouched(SpellCircleLocation circleLocation, SpellCirclePoint point)
    {
        if (lastLocation == circleLocation) return true;
        if (circleLocation == SpellCircleLocation.none) return false;

        if ((element == Element.fire && fireAmount <= edges.Count + 1.99f) ||
            (element == Element.water && waterAmount <= edges.Count + 1.99f) ||
            (element == Element.air && airAmount <= edges.Count + 1.99f) ||
            (element == Element.earth && earthAmount <= edges.Count + 1.99f))
        {
            point.ResetSpellCirclePoint();
            return false;
        }

        if (lastLocation != SpellCircleLocation.none)
        {
            if (edges.Count == 0) UpdateLines(lastLocation);
            edges.Add(new SpellCircleEdge(lastLocation, circleLocation));
            UpdateLines(circleLocation);
        }
        lastLocation = circleLocation;
        return true;
    }

    private void UpdateResources()
    {
        earthAmount = Mathf.Clamp(earthAmount + Time.deltaTime * resourceGainSpeed, 0, 8);
        airAmount = Mathf.Clamp(airAmount + Time.deltaTime * resourceGainSpeed, 0, 8);
        fireAmount = Mathf.Clamp(fireAmount + Time.deltaTime * resourceGainSpeed, 0, 8);
        waterAmount = Mathf.Clamp(waterAmount + Time.deltaTime * resourceGainSpeed, 0, 8);
        earthResource.UpdateResource(earthAmount);
        fireResource.UpdateResource(fireAmount);
        waterResource.UpdateResource(waterAmount);
        airResource.UpdateResource(airAmount);
    }

    private void UpdateLines(SpellCircleLocation circleLocation)
    {
        Vector3[] newLinePositions = new Vector3[linePositions.Length + 1];
        for (int i = 0; i < linePositions.Length; i++)
        {
            newLinePositions[i] = linePositions[i];
        }
        switch (circleLocation)
        {
            case SpellCircleLocation.top:
                newLinePositions[linePositions.Length] = new Vector3(0, 0.196f, 0) / 0.4f;
                break;
            case SpellCircleLocation.topRight:
                newLinePositions[linePositions.Length] = new Vector3(0.1386f, 0.1386f, 0) / 0.4f;
                break;
            case SpellCircleLocation.right:
                newLinePositions[linePositions.Length] = new Vector3(0.196f, 0, 0) / 0.4f;
                break;
            case SpellCircleLocation.bottomRight:
                newLinePositions[linePositions.Length] = new Vector3(0.1386f, -0.1386f, 0) / 0.4f;
                break;
            case SpellCircleLocation.bottom:
                newLinePositions[linePositions.Length] = new Vector3(0, -0.196f, 0) / 0.4f;
                break;
            case SpellCircleLocation.bottomLeft:
                newLinePositions[linePositions.Length] = new Vector3(-0.1386f, -0.1386f, 0) / 0.4f;
                break;
            case SpellCircleLocation.left:
                newLinePositions[linePositions.Length] = new Vector3(-0.196f, 0, 0) / 0.4f;
                break;
            case SpellCircleLocation.topLeft:
                newLinePositions[linePositions.Length] = new Vector3(-0.1386f, 0.1386f, 0) / 0.4f;
                break;
            default:
                newLinePositions[linePositions.Length] = new Vector3(0, 0, 0) / 0.4f;
                break;
        }
        linePositions = newLinePositions;
        line.positionCount = newLinePositions.Length;
        line.SetPositions(linePositions);
    }

    public void CastAttempt()
    {
        if (edges.Count <= 0)
        {
            ResetCasting();
            return;
        }

        foreach (GameObject spellPrefab in spells)
        {
            //Debug.Log("Checking prefab " + spellPrefab);
            if (spellPrefab.GetComponent<Spell>() == null) continue;
            if (spellPrefab.GetComponent<Spell>().GetElementToCast() != element) continue;

            //Debug.Log("Checking prefab's edgesOptions");
            foreach (SpellEdgesOption edgesOption in spellPrefab.GetComponent<Spell>().GetSpellEdgesOptions())
            {
                foreach (SpellCircleEdge spellEdgeInEdges in edges)
                {
                    if (edges.Count != edgesOption.spellCircleEdges.Count)
                    {
                        //Debug.Log("edgesOptions mismatched length " + edges.Count + " " + edgesOption.spellCircleEdges.Count);
                        continue;
                    }

                    //Debug.Log("edgesOption count matched : " + spellPrefab.name);
                    bool cast = true;
                    foreach (SpellCircleEdge e in edgesOption.spellCircleEdges)
                    {
                        if (!SpellCircleEdge.ListContainsSameEdge(edges, e))
                        {
                            // Debug.Log("missing edge " + e.location1 + " & " + e.location2);
                            cast = false;
                            break;
                        }
                    }
                    if (cast)
                    {
                        CastSpell(spellPrefab);
                        return;
                    }
                    //Debug.Log("could not find matching edges with spellprefab " + spellPrefab.name);
                }
            }
        }
        //Debug.Log("No spell cast");
        ResetCasting();
    }

    public void ResetCasting()
    {
        circleImage.color = ColorHelpers.SetColorAlpha(circleImage.color, 1);
        lastTouchedTimer = lastTouchedTimeSet;
        lastLocation = SpellCircleLocation.none;
        visualsGameObject.SetActive(false);
        edges.Clear();
        //ignoreUntilUninput = preparingHand;
        castingHand = null;
        //preparingHand = null;
        foreach (SpellCirclePoint point in spellCirclePoints)
        {
            point.ResetSpellCirclePoint();
        }
        linePositions = new Vector3[0];
        line.positionCount = 0;
        line.SetPositions(linePositions);
    }

    private void CastSpell(GameObject spellPrefab)
    {
        //Debug.Log("CastSpell");
        Instantiate(spellPrefab, visualsGameObject.transform.position, spellPrefab.transform.rotation).GetComponent<Spell>().SpellInit(castingHand);

        // GameObject spellObj = Instantiate(spellPrefab, transform.position, spellPrefab.transform.rotation);
        // Spell spellScript = spellObj.GetComponent<Spell>();
        // spellScript.SpellInit(castingHand);

        switch (element)
        {
            case Element.water:
                waterAmount -= edges.Count + 1;
                break;
            case Element.earth:
                earthAmount -= edges.Count + 1;
                break;
            case Element.fire:
                fireAmount -= edges.Count + 1;
                break;
            case Element.air:
                airAmount -= edges.Count + 1;
                break;
        }

        ResetCasting();
    }

    public void SetElement(Element given)
    {
        lastTouchedTimer = lastTouchedTimeSet;
        element = given;
        Color newColor = Color.white;
        switch (element)
        {
            case Element.water:
                newColor = new Color(0.15f, 0.5f, 0.9f);
                break;
            case Element.earth:
                newColor = new Color(0.7f, 0.5f, 0.2f);
                break;
            case Element.fire:
                newColor = Color.red;
                break;
            case Element.air:
                newColor = new Color(0.85f, 0.95f, 0.95f);
                break;
        }
        circleImage.color = newColor;
        line.startColor = newColor;
        line.endColor = newColor;
    }

    /*public PlayerHand GetCastingHand()
    {
        return castingHand;
    }
    public PlayerHand GetPreparingHand()
    {
        return preparingHand;
    }

    public void SetCastingHand(PlayerHand given)
    {
        castingHand = given;
    }*/

    public void AddSpellCirclePoint(SpellCirclePoint point)
    {
        spellCirclePoints.Add(point);
    }

    public void TouchedByRay()
    {
        lastTouchedTimer = lastTouchedTimeSet;
        circleImage.color = ColorHelpers.SetColorAlpha(circleImage.color, 1);
    }
}

[Serializable]
public enum SpellCircleLocation
{
    none = 0,
    top = 1,
    topRight = 2,
    right = 3,
    bottomRight = 4, 
    bottom = 5,
    bottomLeft = 6, 
    left = 7, 
    topLeft = 8
}

[Serializable]
public struct SpellCircleEdge
{
    public SpellCircleEdge(SpellCircleLocation given1, SpellCircleLocation given2)
    {
        location1 = given1;
        location2 = given2;
    }

    public bool SameEdge(SpellCircleEdge other)
    {
        return (location1 == other.location1 && location2 == other.location2)
            || location2 == other.location1 && location1 == other.location2;
    }

    public static bool ListContainsSameEdge(List<SpellCircleEdge> edges, SpellCircleEdge edge)
    {
        foreach (SpellCircleEdge e in edges)
            if (edge.SameEdge(e))
                return true;
        return false;
    }

    public SpellCircleLocation location1;
    public SpellCircleLocation location2;
}
