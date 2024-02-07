using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : AEffect
{
    [SerializeField] private float destroyDelay = 0.0f;
    // [SerializeField] private List<GameObject> destroyList = new List<GameObject>();

    public override void DoEffect()
    {
        Destroy(gameObject, destroyDelay);
    }
}
