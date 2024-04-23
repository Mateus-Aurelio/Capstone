using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject tree;
    private GameObject goal;
    private EnemyState enemyState = EnemyState.none;
    [SerializeField] private float attackRange = 2;

    void Start()
    {
        tree = GameObject.Find("TreeGoal");
        goal = tree;
        GetComponent<NavMeshAgent>().SetDestination(goal.transform.position);
        SetState(EnemyState.walkToTree);
    }

    private void Update()
    {
        switch (enemyState)
        {
            case EnemyState.none:
                break;
            case EnemyState.walkToTree:
                WalkToTreeUpdate();
                break;
            case EnemyState.walkToPlayer:
                WalkToPlayerUpdate();
                break;
            case EnemyState.attacking:
                AttackUpdate();
                break;
        }
    }

    private void WalkToTreeUpdate()
    {
        if (Vector3.Distance(goal.transform.position, transform.position) <= attackRange)
        {
            SetState(EnemyState.attacking);
        }
    }

    private void WalkToPlayerUpdate()
    {
        if (Vector3.Distance(goal.transform.position, transform.position) <= attackRange)
        {
            SetState(EnemyState.attacking);
        }
    }

    private void AttackUpdate()
    {
        if (Vector3.Distance(goal.transform.position, transform.position) > attackRange +0.1f)
        {
            if (goal == PlayerTracker.GetPlayer())
            {
                SetState(EnemyState.walkToPlayer);
            }
            else
            {
                SetState(EnemyState.walkToTree);
            }
        }
    }

    private void SetState(EnemyState givenState)
    {
        enemyState = givenState;
        switch (enemyState)
        {
            case EnemyState.none:
                goal = null;
                break;
            case EnemyState.walkToTree:
                goal = tree;
                break;
            case EnemyState.walkToPlayer:
                goal = PlayerTracker.GetPlayer();
                break;
            case EnemyState.attacking:
                goal = null;
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
