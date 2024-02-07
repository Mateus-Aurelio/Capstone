using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChange : MonoBehaviour
{
    [SerializeField] private bool newBool = false;
    [SerializeField] private float radius = 3;

    void Start()
    {
        TerrainZone terrain;
        foreach (RaycastHit hit in Physics.SphereCastAll(transform.position, 10, Vector3.down, 10))
        {
            Debug.Log("hit");
            terrain = hit.collider.GetComponent<TerrainZone>();
            if (terrain == null) continue;
            Debug.Log("hit terrain!");
            terrain.ModifyTerrain(transform.position, newBool, radius);
        }
    }
}
