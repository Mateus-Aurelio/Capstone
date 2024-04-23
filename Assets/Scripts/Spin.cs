using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private Vector3 spin = new Vector3(0, 0, 0);

    void Update()
    {
        transform.Rotate(spin * Time.deltaTime);
    }

    public void SetSpinVector(Vector3 given)
    {
        spin = given;
    }
}
