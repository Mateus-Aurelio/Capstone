using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpell : Spell
{
    // [SerializeField] private GameObject prefab;
    [SerializeField] private bool spawnRelativeToBase = true;
    [SerializeField] private bool spawnRelativeToHand = false;
    [SerializeField] private Vector3 relativeSpawnPos;
    [SerializeField] private bool spawnGrounded = true;

    public override void SpellInit(PlayerHand mainHand)
    {
        //Debug.Log("started at " + transform.position + " rot " + transform.rotation.eulerAngles);
        Transform relativeTransform = mainHand.transform;
        if (spawnRelativeToBase)
        {
            relativeTransform = PlayerTracker.GetPlayer().transform;
        }
        else if (spawnRelativeToHand)
        {
            relativeTransform = mainHand.transform;
        }
        transform.position = relativeTransform.position;
        //transform.rotation = Quaternion.Euler(0, mainHand.GetRelativeTransform().rotation.eulerAngles.y, 0);
        //transform.LookAt(transform.position - (Camera.main.transform.position - transform.position));
        // transform.Translate(relativeSpawnPos, transform);
        Transform test = new GameObject().transform;
        relativeTransform = test;
        test.position = PlayerTracker.GetPlayer().transform.position;
        test.LookAt(Camera.main.transform.position + Camera.main.transform.forward * 10);
        //Debug.Log("after LookAt: " + transform.position + " rot " + transform.rotation.eulerAngles);
        transform.rotation = Quaternion.Euler(0, relativeTransform.rotation.eulerAngles.y, 0);
        //Debug.Log("after rotation clamp: " + transform.position + " rot " + transform.rotation.eulerAngles);
        transform.Translate(relativeSpawnPos, transform);

        //Debug.Log("After translate forwards: " + transform.position + " rot " + transform.rotation.eulerAngles);
        if (spawnGrounded)
        {
            Collider[] environmentColliders = Physics.OverlapSphere(transform.position, 20, 1 << LayerMask.NameToLayer("Environment"));
            if (environmentColliders.Length <= 0)
                return;

            Vector3 closestPoint = environmentColliders[0].ClosestPoint(transform.position);
            foreach (Collider c in environmentColliders)
            {
                if (Vector3.Distance(transform.position, c.ClosestPoint(transform.position)) <= Vector3.Distance(transform.position, closestPoint))
                {
                    closestPoint = c.ClosestPoint(transform.position);
                    //Debug.Log("new grounded closest point at" + closestPoint);
                }
            }

            // Debug.Log("grounded to " + environmentColliders[0].gameObject.name);
            /*float minDistance = 99;
            foreach (Collider c in environmentColliders)
            {
                minDistance = Mathf.Clamp(minDistance, Vector3.Distance(c.ClosestPoint(transform.position), transform.position), minDistance);
            }*/
            transform.position = closestPoint;
            //Debug.Log("grounded at " + transform.position + " rot " + transform.rotation.eulerAngles);
        }
    }

}
