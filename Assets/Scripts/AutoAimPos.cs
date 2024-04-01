using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AutoAimPosList
{
    public static List<Transform> autoAimTransforms;
    private static float timeLastCleanedList;
    private static float cleanListWaitTime = 1;

    public static void AddTransform(Transform given)
    {
        if (given == null) return;
        CleanList();
        autoAimTransforms.Add(given);
    }

    public static List<Transform> GetList()
    {
        ForceCleanList();
        return autoAimTransforms;
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
        foreach (Transform t in autoAimTransforms)
        {
            if (t == null || t.gameObject == null) continue;
            newList.Add(t);
        }
        autoAimTransforms = newList;
        timeLastCleanedList = Time.time;
    }

    public static void Reset()
    {
        autoAimTransforms = new List<Transform>();
        timeLastCleanedList = Time.time;
    }
}

public class AutoAimPos : MonoBehaviour
{
    [SerializeField] private bool setUpAutoAimSystem = false;

    void Awake()
    {
        if (setUpAutoAimSystem) SetUpAutoAimSystem();
        StartCoroutine("AddSelf");
    }

    private void SetUpAutoAimSystem()
    {
        AutoAimPosList.Reset();
    }

    private IEnumerator AddSelf()
    {
        yield return new WaitForEndOfFrame();
        AutoAimPosList.AddTransform(transform);
    }
}
