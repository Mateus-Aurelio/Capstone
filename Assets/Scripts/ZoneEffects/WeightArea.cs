using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightArea : CollisionStorer
{
    [SerializeField] private Vector3 lowestPosition;
    [SerializeField] private Vector3 highestPosition;
    private float currentWeight = 0;
    private Rigidbody tempRB;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    protected override void NewObjectCollided(GameObject newObject)
    {
        tempRB = newObject.GetComponent<Rigidbody>();
        if (tempRB == null) return;
        currentWeight += tempRB.mass;
        tempRB = null;
    }

    protected override void NewObjectUncollided(GameObject newObject)
    {
        tempRB = newObject.GetComponent<Rigidbody>();
        if (tempRB == null) return;
        currentWeight -= tempRB.mass;
        tempRB = null;
    }

    protected override void ObjectsCollidingListChanged()
    {

    }
}
