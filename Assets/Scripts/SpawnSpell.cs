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
        transform.rotation = Quaternion.Euler(0, mainHand.GetRelativeTransform().rotation.eulerAngles.y, 0);
        transform.Translate(relativeSpawnPos, transform);
        if (spawnGrounded)
        {
            Collider[] environmentColliders = Physics.OverlapSphere(transform.position, 20, LayerMask.NameToLayer("Environment"));
            if (environmentColliders.Length <= 0) return;
            /*float minDistance = 99;
            foreach (Collider c in environmentColliders)
            {
                minDistance = Mathf.Clamp(minDistance, Vector3.Distance(c.ClosestPoint(transform.position), transform.position), minDistance);
            }*/
            transform.position = environmentColliders[0].ClosestPoint(transform.position);
        }
    }

}
