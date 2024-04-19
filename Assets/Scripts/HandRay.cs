using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandRay : MonoBehaviour
{
    [SerializeField] private PlayerHand hand;
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (hand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
        {
            SendRay();
        }
        else
        {
            lineRenderer.SetPositions(new Vector3[2] { new Vector3(0, 0, 0), new Vector3(0, 0, 1) });
            lineRenderer.endColor = new Color(1, 1, 1, 0);
            lineRenderer.startColor = new Color(1, 1, 1, 0);
        }
    }

    private void SendRay()
    {
        RaycastHit hit;
        int layerMask = 1 << 3;

        lineRenderer.startColor = new Color(1, 1, 1, 1);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 3, layerMask))
        {
            // if (hit.collider.GetComponent<SpellCircle>() != null)
            if (hit.collider.gameObject.name == "SpellCircle Canvas")
            {
                lineRenderer.SetPositions(new Vector3[2] { new Vector3(0, 0, 0), new Vector3(0, 0, Vector3.Distance(transform.position, hit.point)) });
                lineRenderer.endColor = Color.white;
                return;
            }
            else if (hit.collider.GetComponent<SpellCirclePoint>() != null)
            {
                lineRenderer.SetPositions(new Vector3[2] { new Vector3(0, 0, 0), new Vector3(0, 0, Vector3.Distance(transform.position, hit.point)) });
                lineRenderer.endColor = Color.white;
                hit.collider.GetComponent<SpellCirclePoint>().TouchedByRay();
                return;
            }
        }
        lineRenderer.SetPositions(new Vector3[2] { new Vector3(0, 0, 0), new Vector3(0, 0, 1) });
        lineRenderer.endColor = new Color(1, 1, 1, 0);
    }
}
