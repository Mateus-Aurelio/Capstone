using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpell : Spell
{
    // [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject preparedPrefab;
    [SerializeField] private bool spawnRelativeToHandRot = false;
    [SerializeField] private bool spawnRelativeToCameraRot = false;
    [SerializeField] private bool spawnRelativeToFromCameraToHand = false;
    [SerializeField] private Vector3 relativeSpawnPos;
    [SerializeField] private bool spawnGrounded = true;
    [SerializeField] private bool rotateAway = false;

    public override void SpellInit(PlayerHand mainHand)
    {
        DetermineSpawnPos(transform, transform.position, mainHand);
    }

    public override GameObject GetPreparedPrefab(PlayerHand mainHand)
    {
        return preparedPrefab;
    }

    public override void UpdatePreparedObject(Vector3 defaultSpawnPosition, PlayerHand mainHand, GameObject preparedObject)
    {
        DetermineSpawnPos(preparedObject.transform, defaultSpawnPosition, mainHand);
    }

    private void DetermineSpawnPos(Transform givenTransform, Vector3 defaultSpawnPosition, PlayerHand mainHand)
    {
        Vector3 spawnPos = defaultSpawnPosition;
        //Debug.Log("started at " + transform.position + " rot " + transform.rotation.eulerAngles);
        Transform relativeTransform = mainHand.transform;
        spawnPos = relativeTransform.position;
        //transform.rotation = Quaternion.Euler(0, mainHand.GetRelativeTransform().rotation.eulerAngles.y, 0);
        //transform.LookAt(spawnPos - (Camera.main.transform.position - spawnPos));
        // transform.Translate(relativeSpawnPos, transform);
        Transform test = new GameObject().transform;
        Destroy(test.gameObject, 0.001f);
        relativeTransform = test;
        test.position = PlayerTracker.GetPlayer().transform.position;
        if (spawnRelativeToCameraRot)
        {
            test.LookAt(Camera.main.transform.position + Camera.main.transform.forward * 10);
        }
        else if (spawnRelativeToHandRot)
        {
            test.LookAt(mainHand.transform.position + mainHand.transform.forward * 10);
        }
        else if (spawnRelativeToFromCameraToHand)
        {
            test.LookAt(Camera.main.transform.position + (mainHand.transform.position - Camera.main.transform.position) * 10);
        }
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
                givenTransform.position = spawnPos;
                if (rotateAway) givenTransform.rotation = test.rotation;
                return;
            }

            Collider[] environmentColliders = Physics.OverlapSphere(spawnPos, 20, 1 << LayerMask.NameToLayer("Environment"));
            if (environmentColliders.Length <= 0)
            {
                givenTransform.position = spawnPos;
                if (rotateAway) givenTransform.rotation = test.rotation;
                return;
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
        givenTransform.position = spawnPos;
        if (rotateAway) givenTransform.rotation = test.rotation;
        return;
    }
}
