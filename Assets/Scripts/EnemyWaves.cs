using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaves : MonoBehaviour
{
    private int waveNumber = 0;
    [SerializeField] private List<GameObject> wave1 = new List<GameObject>();
    [SerializeField] private List<GameObject> wave2 = new List<GameObject>();
    [SerializeField] private List<GameObject> wave3 = new List<GameObject>();
    [SerializeField] private List<GameObject> wave4 = new List<GameObject>();

    private List<GameObject> livingEnemies = new List<GameObject>();

    float timer = 0;

    private void Start()
    {

    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0) return;
        timer = 1;
        foreach (GameObject g in livingEnemies)
        {
            if (g == null)
            {
                CleanList();
                break;
            }
        }
    }

    private void CleanList()
    {
        List<GameObject> newList = new List<GameObject>();

        foreach (GameObject g in livingEnemies)
            if (g != null) newList.Add(g);
        livingEnemies = newList;
    }
}
