using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Spellcasting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHand leftHand;
    [SerializeField] private PlayerHand rightHand;
    private SpellcastingState spellcastingState = SpellcastingState.none;

    [Header("Pre-Preparation")]
    [SerializeField] private float handsDistanceApartToPrepare = 0.2f;
    [SerializeField] private float handsDistanceFromCoreToPrepare = 0.2f;

    [Header("Preparation")]
    [SerializeField] private float timeToPrepare = 0.5f;
    private float prepareTime = 0.0f;

    [Header("Calling")]
    [SerializeField] private List<Element> elementsAvailable = new List<Element>() { Element.earth };
    private float callingFromYPos;
    [SerializeField] private float distanceFromCoreToCall = 0.3f;

    [Header("Drawing")]
    private Element calledElement;
    private bool leftHandIsDrawing;
    private bool rightHandIsDrawing;
    private SplineContainer rightSpline;
    private SplineContainer leftSpline;
    [SerializeField] private GameObject splinePrefab;
    [SerializeField] private Transform drawnGlyphParent;
    //[Header("Drawn")]
    //[SerializeField] private float x;

    void Start()
    {
        
    }

    void Update()
    {
        // Debug.Log("State: " + spellcastingState);
        if (!BodyData.dataExists)
        {
            // Debug.Log("No body data!");
            return;
        }
        switch (spellcastingState)
        {
            case SpellcastingState.none:
                NoneStateUpdate();
                break;
            case SpellcastingState.preparing:
                PreparingStateUpdate();
                break;
            case SpellcastingState.calling:
                CallingStateUpdate();
                break;
            case SpellcastingState.drawing:
                DrawingStateUpdate();
                break;
            case SpellcastingState.drawn:
                DrawnStateUpdate();
                break;
            default:
                break;
        }
    }

    private void NoneStateUpdate()
    {
        /*Debug.Log("D1: " +( Vector3.Distance(leftHand.GetHandPosition(), rightHand.GetHandPosition()) <= handsDistanceApartToPrepare) +
            "D2: " + (Vector3.Distance(leftHand.GetHandPosition(), BodyData.core) <= handsDistanceFromCoreToPrepare) +
            "D3: " + (Vector3.Distance(rightHand.GetHandPosition(), BodyData.core) <= handsDistanceFromCoreToPrepare)); */
        //Debug.Log("LHP: " + leftHand.GetHandPosition());
        //Debug.Log("RHP: " + rightHand.GetHandPosition());
        //Debug.Log("core: " + BodyData.core);
        if (Vector3.Distance(leftHand.GetHandPosition(), rightHand.GetHandPosition()) <= handsDistanceApartToPrepare
            && Vector3.Distance(leftHand.GetHandPosition(), BodyData.core) <= handsDistanceFromCoreToPrepare
            && Vector3.Distance(rightHand.GetHandPosition(), BodyData.core) <= handsDistanceFromCoreToPrepare)
        {
            prepareTime = 0;
            spellcastingState = SpellcastingState.preparing;
            leftHand.TriggerHaptic(.6f, 0.05f);
            rightHand.TriggerHaptic(.6f, 0.05f);
        }
    }

    private void PreparingStateUpdate()
    {
        prepareTime += Time.deltaTime;
        if (prepareTime >= timeToPrepare)
        {
            spellcastingState = SpellcastingState.calling;
            calledElement = Element.none;
            prepareTime = 0;
            leftHand.TriggerHaptic(0.5f, 0.5f);
            rightHand.TriggerHaptic(0.5f, 0.5f);
            callingFromYPos = (leftHand.GetHandPosition().y + rightHand.GetHandPosition().y) / 2;
        } 
    }

    private void CallingStateUpdate()
    {
        if (Vector3.Distance(leftHand.GetHandPosition(), BodyData.core) >= distanceFromCoreToCall
            && Vector3.Distance(rightHand.GetHandPosition(), BodyData.core) >= distanceFromCoreToCall)
        {
            foreach (Element e in elementsAvailable)
            {
                if (ElementsCallShape.ElementCalled(e, leftHand.GetHandPosition(), rightHand.GetHandPosition()))
                {
                    spellcastingState = SpellcastingState.drawing;
                    calledElement = e;
                    //Debug.Log("Called " + e + "!");
                    leftHand.TriggerHaptic(0.99f, 0.4f);
                    rightHand.TriggerHaptic(0.99f, 0.4f);
                    return;
                }
            }
        }
    }

    private void DrawingStateUpdate()
    {
        //Debug.Log("Drawing");
        if (rightHand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
        {
            //Debug.Log("Right trigger down");
            if (!rightHandIsDrawing)
            {
                //Debug.Log("Starting spline!");
                rightHandIsDrawing = true;
                rightSpline = Instantiate(splinePrefab, rightHand.transform.position, splinePrefab.transform.rotation, drawnGlyphParent).GetComponent<SplineContainer>();
                rightSpline.Spline.Clear();
            }
            //Debug.Log("Adding to spline");
            rightSpline.Spline.Add(new BezierKnot(rightHand.GetHandPosition()));
        }
        else if (rightHandIsDrawing)
        {
            //Debug.Log("Done drawing");
            rightHandIsDrawing = false;
            // finish spline, add it to list of splines?
        }

        if (leftHand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
        {
            
        }
    }

    private void DrawnStateUpdate()
    {

    }

}

public enum SpellcastingState
{
    none = 0,
    preparing = 1, // hands are near chest, preparing to call an element 
    calling = 2, // moving after preparing, selecting an element 
    drawing = 3, // able to draw a glyph
    drawn = 4 // a glyph has been drawn
}

public static class ElementsCallShape
{
    public static bool ElementCalled(Element element, Vector3 leftHandPos, Vector3 rightHandPos)
    {
        switch (element)
        {
            case Element.air:
                return false;
            case Element.water:
                return false;
            case Element.fire:
                return false;
            case Element.earth:
                float distanceDown = Vector3.Distance(BodyData.shouldersCenter, BodyData.core);
                /*Debug.Log("Calling earth? distanceDown: " + distanceDown +
                    " LHP: " + leftHandPos +
                    " RHP: " + rightHandPos +
                    " core: " + BodyData.core);*/
                //Debug.Log();
                //Debug.Log();
                //Debug.Log();
                return leftHandPos.y <= BodyData.core.y - distanceDown * .5f && rightHandPos.y <= BodyData.core.y - distanceDown * .5f;
            default:
                return false;
        }
    }
}