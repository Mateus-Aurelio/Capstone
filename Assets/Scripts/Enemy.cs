using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject goal;
    private EnemyState enemyState = EnemyState.none;
    [SerializeField] private float attackRange = 2; 

    void Start()
    {
        goal = GameObject.Find("TreeGoal");
        GetComponent<NavMeshAgent>().SetDestination(goal.transform.position);
    }

    private void Update()
    {
        switch (enemyState)
        {
            case EnemyState.none:
                break;
            case EnemyState.walkToTree:
                break;
            case EnemyState.walkToPlayer:
                break;
            case EnemyState.attacking:
                break;
        }
    }
}

public enum EnemyState
{
    none = 0, 
    walkToTree = 1, 
    walkToPlayer = 2, 
    attacking = 3
}
