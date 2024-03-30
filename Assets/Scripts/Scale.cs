using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale : MonoBehaviour
{
    [SerializeField] private Transform leftPlate;
    [SerializeField] private SpringJoint leftSpring;
    [SerializeField] private Transform rightPlate; 
    [SerializeField] private SpringJoint rightSpring; 
    [SerializeField] private Vector2 leftRightNeutralYPositions; 
    private Vector2 yPositions; 

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
