using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject goal;
    void Start()
    {
        GetComponent<NavMeshAgent>().SetDestination(goal.transform.position);
    }

}
