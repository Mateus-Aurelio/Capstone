using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardOverTime : MonoBehaviour
{
    [SerializeField] private float velocity;
    [SerializeField] private float accel;

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * velocity * Time.fixedDeltaTime, transform);
        velocity += accel * Time.fixedDeltaTime;
    }

    public void SetVelocity(float given)
    {
        velocity = given;
    }

    public float GetVelocity()
    {
        return velocity;
    }
}
