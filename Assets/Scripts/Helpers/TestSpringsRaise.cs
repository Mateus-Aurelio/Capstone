using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpringsRaise : MonoBehaviour
{
    [SerializeField] private Transform body1;
    [SerializeField] private Transform body2;
    [SerializeField] private float speed = 10;
    private float move;
    
    void Update()
    {
        move = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;
        body1.Translate(Vector3.up * move);
        body2.Translate(Vector3.down * move);
    }
}
