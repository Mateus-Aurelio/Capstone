using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpell : Spell
{
    // [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject preparedPrefab;
    [SerializeField] private bool spawnRelativeToBase = true;
    [SerializeField] private bool spawnRelativeToHand = false;
    [SerializeField] private Vector3 relativeSpawnPos;
    [SerializeField] private bool spawnGrounded = true;

    public override void SpellInit(PlayerHand mainHand)
    {
        transform.position = DetermineSpawnPos(transform.position, mainHand);
    }

    public override GameObject GetPreparedPrefab(PlayerHand mainHand)
    {
        return preparedPrefab;
    }

    public override void UpdatePreparedObject(Vector3 defaultSpawnPosition, PlayerHand mainHand, GameObject preparedObject)
    {
        preparedObject.transform.position = DetermineSpawnPos(defaultSpawnPosition, mainHand);
        preparedObject.transform.LookAt(Camera.main.transform.position);
    }

    private Vector3 DetermineSpawnPos(Vector3 defaultSpawnPosition, PlayerHand mainHand)
    {
        Vector3 spawnPos = defaultSpawnPosition;
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
        spawnPos = relativeTransform.position;
        //transform.rotation = Quaternion.Euler(0, mainHand.GetRelativeTransform().rotation.eulerAngles.y, 0);
        //transform.LookAt(spawnPos - (Camera.main.transform.position - spawnPos));
        // transform.Translate(relativeSpawnPos, transform);
        Transform test = new GameObject().transform;
        Destroy(test.gameObject, 0.001f);
        relativeTransform = test;
        test.position = PlayerTracker.GetPlayer().transform.position;
        test.LookAt(Camera.main.transform.position + Camera.main.transform.forward * 10);
        //Debug.Log("after LookAt: " + transform.position + " rot " + transform.rotation.eulerAngles);
        test.rotation = Quaternion.Euler(0, relativeTransform.rotation.eulerAngles.y, 0);
        //Debug.Log("after rotation clamp: " + spawnPos + " rot " + transform.rotation.eulerAngles);
        test.Translate(relativeSpawnPos, test);
        spawnPos = test.transform.position;

        //Debug.Log("After translate forwards: " + spawnPos + " rot " + transform.rotation.eulerAngles);
        if (spawnGrounded)
        {
            RaycastHit hit;
            if (Physics.Raycast(spawnPos + new Vector3(0, 10, 0), new Vector3(0, -1, 0), out hit, 30, 1 << LayerMask.NameToLayer("Environment")))
            {
                spawnPos = hit.point;
                return spawnPos;
            }

            Collider[] environmentColliders = Physics.OverlapSphere(spawnPos, 20, 1 << LayerMask.NameToLayer("Environment"));
            if (environmentColliders.Length <= 0)
            {
                return spawnPos;
            }

            Vector3 closestPoint = environmentColliders[0].ClosestPoint(spawnPos);
            foreach (Collider c in environmentColliders)
            {
                if (Vector3.Distance(spawnPos, c.ClosestPoint(spawnPos)) <= Vector3.Distance(spawnPos, closestPoint))
                {
                    closestPoint = c.ClosestPoint(spawnPos);
                    //Debug.Log("new grounded closest point at" + closestPoint);
                }
            }

            // Debug.Log("grounded to " + environmentColliders[0].gameObject.name);
            /*float minDistance = 99;
            foreach (Collider c in environmentColliders)
            {
                minDistance = Mathf.Clamp(minDistance, Vector3.Distance(c.ClosestPoint(spawnPos), spawnPos), minDistance);
            }*/
            spawnPos = closestPoint;
            //Debug.Log("grounded at " + spawnPos + " rot " + transform.rotation.eulerAngles);
        }
        return spawnPos;
    }
}
