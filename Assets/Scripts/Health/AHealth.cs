using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AHealth : MonoBehaviour
{
    public abstract void Damage(float given);
    public abstract void Heal(float given);
}
 