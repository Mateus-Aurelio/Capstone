using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamageModifier
{
    public DamageType damageType = DamageType.none;
    public float modifier = 1.0f;
}
