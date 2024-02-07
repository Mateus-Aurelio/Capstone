using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDestroy : MonoBehaviour
{
    [SerializeField] float delayTime;

    void Start()
    {
        Destroy(gameObject, delayTime);
    }
}
