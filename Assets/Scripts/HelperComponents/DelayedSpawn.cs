using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedSpawn : MonoBehaviour
{
    [SerializeField] private float seconds;
    [SerializeField] private GameObject prefab;
    [SerializeField] private bool destroyAfterSpawn;
    [SerializeField] private float randomRangeSeconds;

    void Start()
    {
        Invoke("Spawn", Random.Range(seconds - randomRangeSeconds/2, seconds + randomRangeSeconds/2));
    }

    private void Spawn()
    {
        Instantiate(prefab, transform.position, transform.rotation);
        if (destroyAfterSpawn) Destroy(gameObject);
    }
}
