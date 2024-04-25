using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCircle : MonoBehaviour
{
    private SpellCircleLocation lastLocation = SpellCircleLocation.none;
    private List<SpellCircleEdge> edges = new List<SpellCircleEdge>();
    [SerializeField] private GameObject visualsGameObject; // also where a spell defaults to being cast
    [SerializeField] private bool unparentSpellCircleOnEnterCastMode = false;
    [SerializeField] private Transform parent;
    [SerializeField] private Vector3 parentRelativeOffset;
    private Spell currentPreparedSpellScript;
    private GameObject preparedSpellCastVisual;
    [SerializeField] private Image circleImage;
    [SerializeField] private Image circleImageInner;
    [SerializeField] private Image circleImageInnerText;
    [SerializeField] private Image circleImageOuterText;
    [SerializeField] private List<Image> imagesToColor = new List<Image>();
    private float textFadeInTimer;

    [SerializeField] private Light pointLight;
    [SerializeField] private AlternateImage introAnimInner;
    [SerializeField] private AlternateImage introAnimOuter;

    [SerializeField] private LineRenderer line;
    private Vector3[] linePositions = new Vector3[0];
    [SerializeField] private Transform playerCamera;

    [SerializeField] private List<GameObject> spells = new List<GameObject>();
    private List<SpellCirclePoint> spellCirclePoints = new List<SpellCirclePoint>();

    private PlayerHand castingHand = null;
    [SerializeField] private PlayerHand leftHand = null;
    [SerializeField] private PlayerHand rightHand = null;
    private bool ignoreUntilUninput = false;

    private Element element = Element.earth;
    [SerializeField] private bool setElementToNoneOnReset = false;

    [SerializeField] private Color fireColor = Color.red;
    [SerializeField] private Color waterColor = Color.blue;
    [SerializeField] private Color airColor = Color.white;
    [SerializeField] private Color earthColor = Color.black;
    [SerializeField] private float innerFadeAlpha = 0.2f;

    private void Awake()
    {
        SetElement(element);
        ResetCasting();
    }
    

    private void Update()
    {
        /*if (lastTouchedTimer > 0)
        {
            lastTouchedTimer -= Time.deltaTime;
            if (lastTouchedTimer <= 0)
                circleImage.color = ColorHelpers.SetColorAlpha(circleImage.color, circleImageMinAlpha);
            else if (lastTouchedTimer < lastTouchFadeTime)
                circleImage.color = ColorHelpers.SetColorAlpha(circleImage.color, circleImageMinAlpha + lastTouchedTimer);
        }*/
        
        //UpdateResources();
        /*if (lastLocation == SpellCircleLocation.none)
        {
            if (VRInput.ButtonPressed(UnityEngine.XR.XRNode.LeftHand, UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.PrimaryButton)) SetElement(Element.earth);
            else if (VRInput.ButtonPressed(UnityEngine.XR.XRNode.LeftHand, UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.SecondaryButton)) SetElement(Element.air);
            else if (VRInput.ButtonPressed(UnityEngine.XR.XRNode.RightHand, UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.PrimaryButton)) SetElement(Element.water);
            else if (VRInput.ButtonPressed(UnityEngine.XR.XRNode.RightHand, UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.SecondaryButton)) SetElement(Element.fire);
        }*/

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
            // ENABLE SPELLCASTING
            introAnimInner.ResetAnim();
            rightHand.SetCasting(true);
            visualsGameObject.SetActive(true);
            ignoreUntilUninput = true;
            textFadeInTimer = 0;
            if (unparentSpellCircleOnEnterCastMode)
            {
                //visualsGameObject.transform.parent = parent;
                /*visualsGameObject.transform.SetParent(parent);
                visualsGameObject.transform.rotation = parent.rotation;
                visualsGameObject.transform.Translate(parentRelativeOffset);
                visualsGameObject.transform.SetParent(null);*/

                transform.SetParent(parent);
                transform.localPosition = parentRelativeOffset;
                //transform.Translate(parentRelativeOffset, parent);
                // transform.position = parent.position + Vector3.Project(parentRelativeOffset, parent.forward);
                transform.SetParent(PlayerTracker.GetPlayer().transform);
                //visualsGameObject.transform.parent = null;
            }
            // transform.position = leftHand.transform.position;
        }

        visualsGameObject.transform.LookAt(visualsGameObject.transform.position - (playerCamera.position - visualsGameObject.transform.position));
        textFadeInTimer += Time.deltaTime;
        circleImageInnerText.color = ColorHelpers.SetColorAlpha(circleImageInnerText.color, circleImageInnerText.color.a + Time.deltaTime);
        if (element != Element.none) circleImageOuterText.color = ColorHelpers.SetColorAlpha(circleImageOuterText.color, circleImageOuterText.color.a + Time.deltaTime);
        castingHand = rightHand;
        // if (castingHand == null) return; 

        if (currentPreparedSpellScript != null)
        {
            UpdatePreparedSpellVisuals();
        }

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

        /*if ((element == Element.fire && fireAmount <= edges.Count + 1.99f) ||
            (element == Element.water && waterAmount <= edges.Count + 1.99f) ||
            (element == Element.air && airAmount <= edges.Count + 1.99f) ||
            (element == Element.earth && earthAmount <= edges.Count + 1.99f))
        {
            point.ResetSpellCirclePoint();
            return false;
        }*/

        if (lastLocation != SpellCircleLocation.none)
        {
            if (edges.Count == 0) UpdateLines(lastLocation);
            edges.Add(new SpellCircleEdge(lastLocation, circleLocation));
            UpdateLines(circleLocation);
        }
        lastLocation = circleLocation;
        SetPreparedSpell();
        if (currentPreparedSpellScript != null) UpdatePreparedSpellVisuals();
        return true;
    }

    /*private void UpdateResources()
    {
        earthAmount = Mathf.Clamp(earthAmount + Time.deltaTime * resourceGainSpeed, 0, 8);
        airAmount = Mathf.Clamp(airAmount + Time.deltaTime * resourceGainSpeed, 0, 8);
        fireAmount = Mathf.Clamp(fireAmount + Time.deltaTime * resourceGainSpeed, 0, 8);
        waterAmount = Mathf.Clamp(waterAmount + Time.deltaTime * resourceGainSpeed, 0, 8);
        earthResource.UpdateResource(earthAmount);
        fireResource.UpdateResource(fireAmount);
        waterResource.UpdateResource(waterAmount);
        airResource.UpdateResource(airAmount);
    }*/

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

    private void SetPreparedSpell()
    {
        GameObject spellToCast = DetermineSpellFromEdges();
        if (spellToCast == null)
        {
            currentPreparedSpellScript = null;
            if (preparedSpellCastVisual != null) Destroy(preparedSpellCastVisual);
            return;
        }
        currentPreparedSpellScript = spellToCast.GetComponent<Spell>();
        if (preparedSpellCastVisual != null) Destroy(preparedSpellCastVisual);
        preparedSpellCastVisual = Instantiate(currentPreparedSpellScript.GetPreparedPrefab(castingHand));
        UpdatePreparedSpellVisuals();
    }

    private void UpdatePreparedSpellVisuals()
    {
        currentPreparedSpellScript.UpdatePreparedObject(visualsGameObject.transform.position, castingHand, preparedSpellCastVisual);
    }

    public void CastAttempt()
    {
        if (edges.Count <= 0)
        {
            ResetCasting();
            return;
        }
        GameObject spell = DetermineSpellFromEdges();
        if (spell != null)
        {
            CastSpell(spell);
            return;
        }
        
        //Debug.Log("No spell cast");
        ResetCasting();
    }

    private GameObject DetermineSpellFromEdges()
    {
        foreach (GameObject spellPrefab in spells)
        {
            //Debug.Log("Checking prefab " + spellPrefab);
            if (spellPrefab.GetComponent<Spell>() == null) continue;
            if (element != Element.none && spellPrefab.GetComponent<Spell>().GetElementToCast() != Element.none && spellPrefab.GetComponent<Spell>().GetElementToCast() != element) continue;

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
                        return spellPrefab;
                    }
                    //Debug.Log("could not find matching edges with spellprefab " + spellPrefab.name);
                }
            }
        }
        return null;
    }

    public void ResetCasting()
    {
        rightHand.SetCasting(false);
        currentPreparedSpellScript = null;
        textFadeInTimer = 0;
        if (preparedSpellCastVisual != null)
        {
            Destroy(preparedSpellCastVisual);
            preparedSpellCastVisual = null;
        }
        //lastTouchedTimer = lastTouchedTimeSet;
        lastLocation = SpellCircleLocation.none;
        visualsGameObject.SetActive(false);
        edges.Clear();
        //ignoreUntilUninput = preparingHand;
        castingHand = null;
        //preparingHand = null;
        if (setElementToNoneOnReset) SetElement(Element.none);
        foreach (Image i in imagesToColor) i.color = ColorHelpers.SetColorAlpha(i.color, 0);
        circleImage.color = ColorHelpers.SetColorAlpha(circleImage.color, 0);
        circleImageInner.color = ColorHelpers.SetColorAlpha(circleImageInner.color, 1f);
        circleImageInnerText.color = ColorHelpers.SetColorAlpha(circleImageInnerText.color, 0);
        circleImageOuterText.color = ColorHelpers.SetColorAlpha(circleImageOuterText.color, 0);
        pointLight.color = ColorHelpers.SetColorAlpha(pointLight.color, 1f);
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

        /*switch (element)
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
        }*/

        ResetCasting();
    }

    public Element GetElement()
    {
        return element;
    }

    public void SetElement(Element given)
    {
        //lastTouchedTimer = lastTouchedTimeSet;
        element = given;
        Color newColor = Color.white;
        foreach (Image i in imagesToColor) i.color = ColorHelpers.SetColorAlpha(i.color, 1);
        circleImage.color = ColorHelpers.SetColorAlpha(circleImage.color, 1);
        circleImageInner.color = ColorHelpers.SetColorAlpha(circleImageInner.color, innerFadeAlpha);
        // circleImageInnerText.color = ColorHelpers.SetColorAlpha(circleImageInnerText.color, innerFadeAlpha);
        // circleImageOuterText.color = ColorHelpers.SetColorAlpha(circleImageOuterText.color, innerFadeAlpha);
        pointLight.color = ColorHelpers.SetColorAlpha(pointLight.color, innerFadeAlpha);
        introAnimOuter.ResetAnim();
        switch (element)
        {
            case Element.water:
                newColor = waterColor;
                break;
            case Element.earth:
                newColor = earthColor;
                break;
            case Element.fire:
                newColor = fireColor;
                break;
            case Element.air:
                newColor = airColor;
                break;
            case Element.none:
                circleImage.color = ColorHelpers.SetColorAlpha(circleImage.color, 0.0f);
                circleImageInner.color = ColorHelpers.SetColorAlpha(circleImageInner.color, 1f);
                circleImageInnerText.color = ColorHelpers.SetColorAlpha(circleImageInnerText.color, 1f);
                circleImageOuterText.color = ColorHelpers.SetColorAlpha(circleImageOuterText.color, 1f);
                pointLight.color = ColorHelpers.SetColorAlpha(pointLight.color, 1f);
                break;
        }
        foreach (Image i in imagesToColor) i.color = ColorHelpers.SetColorRGB(i.color, newColor);
        circleImage.color = ColorHelpers.SetColorRGB(circleImage.color, newColor);
        circleImageInner.color = ColorHelpers.SetColorRGB(circleImageInner.color, newColor);
        circleImageInnerText.color = ColorHelpers.SetColorRGB(circleImageInnerText.color, newColor);
        circleImageOuterText.color = ColorHelpers.SetColorRGB(circleImageOuterText.color, newColor);
        pointLight.color = ColorHelpers.SetColorRGB(pointLight.color, newColor);
        line.startColor = newColor;
        line.endColor = newColor;
        foreach (SpellCirclePoint point in spellCirclePoints)
        {
            point.ElementSet();
        }
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
        // lastTouchedTimer = lastTouchedTimeSet;
        // circleImage.color = ColorHelpers.SetColorAlpha(circleImage.color, 1);
    }

    public SpellCircleLocation GetLastLocation()
    {
        return lastLocation;
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
