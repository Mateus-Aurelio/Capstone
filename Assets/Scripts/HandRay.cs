using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandRay : MonoBehaviour
{
    [SerializeField] private PlayerHand hand;
    private LineRenderer lineRenderer;

    [SerializeField] private Gradient defaultGradient; 
    [SerializeField] private Gradient drawingGradient; 
    [SerializeField] private Gradient offGradient;
    private bool drawing = false;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (hand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
        {
            SendRay();
            if (!drawing)
            {
                drawing = true;
            }
        }
        else if (drawing)
        {
            drawing = false;
            lineRenderer.SetPositions(new Vector3[2] { new Vector3(0, 0, 0), new Vector3(0, 0, 0.75f) });
            lineRenderer.colorGradient = offGradient;
        }
    }

    private void SendRay()
    {
        RaycastHit hit;
        int layerMask = 1 << 3;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 3, layerMask))
        {
            // if (hit.collider.GetComponent<SpellCircle>() != null)
            if (hit.collider.gameObject.name == "SpellCircle Canvas")
            {
                Debug.Log("Hit canvas");
                lineRenderer.SetPositions(new Vector3[2] { new Vector3(0, 0, 0), new Vector3(0, 0, Vector3.Distance(transform.position, hit.point)) });
                lineRenderer.colorGradient = drawingGradient;
                return;
            }
            else if (hit.collider.GetComponent<SpellCirclePoint>() != null)
            {
                Debug.Log("Hit point!");
                lineRenderer.SetPositions(new Vector3[2] { new Vector3(0, 0, 0), new Vector3(0, 0, Vector3.Distance(transform.position, hit.point)) });
                lineRenderer.colorGradient = drawingGradient;
                hit.collider.GetComponent<SpellCirclePoint>().TouchedByRay();
                return;
            }
        }
        lineRenderer.SetPositions(new Vector3[2] { new Vector3(0, 0, 0), new Vector3(0, 0, 1) });
        lineRenderer.colorGradient = defaultGradient;
    }
}
