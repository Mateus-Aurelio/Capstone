using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveEffect : AEffect
{
    [SerializeField] private GameObject objectToSet;
    [SerializeField] private bool active;

    public override void DoEffect()
    {
        objectToSet.SetActive(active);
    }
}
