using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private bool wait = true;
    private Transform goal;
    private EnemyState enemyState = EnemyState.none;
    [SerializeField] private float attackRange = 2;

    [SerializeField] private Spin spinWhenAttack;
    [SerializeField] private Vector3 spinVectorNormal = new Vector3(0, 0, -400);
    [SerializeField] private Vector3 spinVectorAttacking = new Vector3(0, 0, -600);

    void Start()
    {
        wait = true;
        StartCoroutine("FindTree");
    }

    private void Update()
    {
        if (wait) return;
        if (goal == null) DetermineGoal();
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

    private void DetermineGoal()
    {
        SetState(EnemyState.walkToTree);
    }

    private void SetState(EnemyState givenState)
    {
        enemyState = givenState;
        if (spinWhenAttack != null) spinWhenAttack.SetSpinVector(spinVectorNormal);
        switch (enemyState)
        {
            case EnemyState.none:
                goal = null;
                break;
            case EnemyState.walkToTree:
                goal = TreePosList.GetList()[0];
                break;
            case EnemyState.walkToPlayer:
                goal = PlayerTracker.GetPlayer().transform;
                break;
            case EnemyState.attacking:
                if (spinWhenAttack != null) spinWhenAttack.SetSpinVector(spinVectorAttacking);
                goal = null;
                break;
        }
    }

    private IEnumerator FindTree()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        goal = TreePosList.GetList()[0];
        GetComponent<NavMeshAgent>().SetDestination(goal.position);
        SetState(EnemyState.walkToTree);
        wait = false;
    }
}

public enum EnemyState
{
    none = 0,
    walkToTree = 1,
    walkToPlayer = 2,
    attacking = 3
}
