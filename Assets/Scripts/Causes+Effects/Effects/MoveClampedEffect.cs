using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveClampedEffect : AEffect
{
    [SerializeField] private bool framerateIndependent = true;
    [SerializeField] private Vector3 moveSpeed;
    [SerializeField] private Vector3 localMinPos;
    [SerializeField] private Vector3 localMaxPos;
    private Vector3 realMove;

    public override void DoEffect()
    {
        realMove = moveSpeed;
        if (framerateIndependent) realMove *= Time.deltaTime;
        transform.Translate(realMove);
        transform.localPosition = new Vector3(
            Mathf.Clamp(transform.localPosition.x, localMinPos.x, localMaxPos.x),
            Mathf.Clamp(transform.localPosition.y, localMinPos.y, localMaxPos.y),
            Mathf.Clamp(transform.localPosition.z, localMinPos.z, localMaxPos.z));
    }
}
