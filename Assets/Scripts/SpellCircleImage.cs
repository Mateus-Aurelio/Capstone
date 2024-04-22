using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCircleImage : MonoBehaviour
{
    [SerializeField] private SpellCircle spellCircle;

    public void TouchedByRay()
    {
        spellCircle.TouchedByRay();
    }
}
