using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ZoneEffect : MonoBehaviour
{
    public abstract void DoEnterEffect(string name);
    public abstract void DoExitEffect(string name);
}
