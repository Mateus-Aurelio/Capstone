using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinEffect : AEffect
{
    [SerializeField] private Transform rotateAroundPoint;
    [SerializeField] private Vector3 rotateAxis;
    [SerializeField] private float rotationalSpeed;
    [SerializeField] private bool framerateIndependent = true;
    private float realSpeed;

    public override void DoEffect()
    {
        realSpeed = rotationalSpeed;
        if (framerateIndependent) realSpeed *= Time.deltaTime;
        transform.RotateAround(rotateAroundPoint.position, rotateAxis, rotationalSpeed);
    }
}
