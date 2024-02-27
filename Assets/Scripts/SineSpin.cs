using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineSpin : MonoBehaviour
{
    private Vector3 centerPos;
    // [SerializeField] private float yBounceModifier = 1;
    [SerializeField] private Vector3 rotationWaves = new Vector3(0f, 0f, 0f);
    [SerializeField] private int rotationOffset = 5;

    void Start()
    {
        centerPos = transform.position;
    }

    private void Update()
    {
        transform.Rotate(Mathf.Sin(Time.time * rotationWaves.x), Mathf.Sin(Time.time * rotationWaves.y), Mathf.Sin(Time.time * rotationWaves.z) * rotationOffset);
        // transform.position = new Vector3(centerPos.x, yBounceModifier * Mathf.Sin(Time.time) + centerPos.y, centerPos.z);
    }
}
