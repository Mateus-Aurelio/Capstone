using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOverTime : MonoBehaviour
{
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 accel;

    void FixedUpdate()
    {
        transform.Translate(velocity * Time.fixedDeltaTime);
        if (accel != Vector3.zero) velocity += accel * Time.fixedDeltaTime;
    }

    public void SetVelocity(Vector3 given)
    {
        velocity = given;
    }
}
