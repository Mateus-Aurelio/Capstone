using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : AHealthTracker
{
    private bool wait = true;
    private Transform goal;
    private EnemyState enemyState = EnemyState.none;
    private NavMeshAgent agent;
    [SerializeField] private float attackRange = 2;
    [SerializeField] private float loseInterestFromPlayerRange = 15;

    [SerializeField] private Spin spinWhenAttack;
    [SerializeField] private Vector3 spinVectorNormal = new Vector3(0, 0, -400);
    [SerializeField] private Vector3 spinVectorAttacking = new Vector3(0, 0, -600);

    [SerializeField] private GameObject faceWhenAttackingTree;
    [SerializeField] private GameObject faceWhenAttackingPlayer;

    [SerializeField] private bool targetNearestTreeFirst = true;
    [SerializeField] private bool targetedNearestTree = false;
    [SerializeField] private bool targetPlayerWhenDamaged = true;
    [SerializeField] private bool targetMainTreeAfterOtherTree = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        faceWhenAttackingTree.SetActive(true);
        faceWhenAttackingPlayer.SetActive(false);
        wait = true;
        StartCoroutine("FindTree");
    }

    private void Update()
    {
        if (wait) return;
        if (goal == null || goal == transform) DetermineGoal();
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
        agent.SetDestination(goal.transform.position);
        if (Vector3.Distance(goal.transform.position, transform.position) <= attackRange)
        {
            SetState(EnemyState.attacking);
        }
        else if (Vector3.Distance(goal.transform.position, transform.position) >= loseInterestFromPlayerRange)
        {
            SetState(EnemyState.walkToTree);
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
                goal = transform;
                break;
            case EnemyState.walkToTree:
                faceWhenAttackingTree.SetActive(true);
                faceWhenAttackingPlayer.SetActive(false);
                if (targetNearestTreeFirst && !targetedNearestTree)
                {
                    goal = GetNearestTree();
                    targetedNearestTree = true;
                }
                else
                {
                    goal = TreePosList.GetMainTree();
                }
                break;
            case EnemyState.walkToPlayer:
                faceWhenAttackingTree.SetActive(false);
                faceWhenAttackingPlayer.SetActive(true);
                goal = PlayerTracker.GetPlayer().transform;
                break;
            case EnemyState.attacking:
                if (spinWhenAttack != null) spinWhenAttack.SetSpinVector(spinVectorAttacking);
                agent.SetDestination(transform.position);
                return;
                // break;
        }
        agent.SetDestination(goal.position);
    }

    private IEnumerator FindTree()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        SetState(EnemyState.walkToTree);
        wait = false;
    }

    private Transform GetNearestTree()
    {
        float minDistance = 999;
        Transform nearest = null;
        int i = 0;
        foreach (Transform t in TreePosList.GetList())
        {
            i++;
            if (Vector3.Distance(t.position, transform.position) < minDistance)
            {
                minDistance = Vector3.Distance(t.position, transform.position);
                nearest = t;
            }
        }
        return nearest;
    }

    public override void HealthChanged(AHealth healthScript)
    {
        SetState(EnemyState.walkToPlayer);
    }
}

public enum EnemyState
{
    none = 0,
    walkToTree = 1,
    walkToPlayer = 2,
    attacking = 3
}
