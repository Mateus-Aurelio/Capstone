using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : AHealthTracker
{
    [SerializeField] private bool mainTree = false;
    [SerializeField] private bool treeListSetup = false;

    private TreeState treeState;
    [SerializeField] private GameObject normalTrunk;
    [SerializeField] private GameObject cutTrunk;
    [SerializeField] private List<GameObject> leaves = new List<GameObject>();

    [SerializeField] private GameObject treeDeathPrefab;

    void Awake()
    {
        treeState = TreeState.healthy;
        if (treeListSetup) SetUpTreeListSystem();
        StartCoroutine("AddSelf");
        if (mainTree) StartCoroutine("AddSelfMain");
    }

    public override void HealthChanged(AHealth healthScript)
    {
        if (treeState == TreeState.healthy && healthScript.GetHealthRatio() <= 0.8f)
        {
            normalTrunk.SetActive(false);
            cutTrunk.SetActive(true);
            treeState = TreeState.hurt;
        }
        else if (treeState == TreeState.hurt && healthScript.GetHealthRatio() <= 0.4f)
        {
            foreach (GameObject g in leaves)
            {
                g.SetActive(false);
            }
            // transform.localScale = new Vector3(transform.localScale.x * 0.8f, transform.localScale.y, transform.localScale.z * 0.8f); 
            treeState = TreeState.veryHurt;
        }
        if (healthScript.GetHealth() <= 0)
        {
            if (treeDeathPrefab != null) Instantiate(treeDeathPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void SetUpTreeListSystem()
    {
        TreePosList.Reset();
    }

    private IEnumerator AddSelf()
    {
        yield return new WaitForEndOfFrame();
        TreePosList.AddTransform(transform);
    }

    private IEnumerator AddSelfMain()
    {
        yield return new WaitForEndOfFrame();
        TreePosList.AddMainTree(transform);
    }
}

public enum TreeState
{
    healthy = 0,
    hurt = 1,
    veryHurt = 2,
}


public static class TreePosList
{
    public static List<Transform> treeTransforms;
    public static Transform mainTreeTransform;
    private static float timeLastCleanedList;
    private static float cleanListWaitTime = 1;

    public static void AddTransform(Transform given)
    {
        if (given == null) return;
        CleanList();
        treeTransforms.Add(given);
    }

    public static List<Transform> GetList()
    {
        ForceCleanList();
        return treeTransforms;
    }

    public static void ForceCleanList()
    {
        timeLastCleanedList = 0;
        CleanList();
    }

    public static void CleanList()
    {
        if (Time.time < timeLastCleanedList + cleanListWaitTime) return;
        List<Transform> newList = new List<Transform>();
        foreach (Transform t in treeTransforms)
        {
            if (t == null || t.gameObject == null) continue;
            newList.Add(t);
        }
        treeTransforms = newList;
        timeLastCleanedList = Time.time;
    }

    public static void Reset()
    {
        treeTransforms = new List<Transform>();
        timeLastCleanedList = Time.time;
        mainTreeTransform = null;
    }

    public static void AddMainTree(Transform mainTree)
    {
        mainTreeTransform = mainTree;
    }

    public static Transform GetMainTree()
    {
        return mainTreeTransform;
    }
}
