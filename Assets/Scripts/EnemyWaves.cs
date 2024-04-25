using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyWaves : MonoBehaviour
{
    private int waveNumber = 0;
    [SerializeField] private List<GameObject> wave1 = new List<GameObject>();
    [SerializeField] private List<GameObject> wave2 = new List<GameObject>();
    [SerializeField] private List<GameObject> wave3 = new List<GameObject>();
    [SerializeField] private List<GameObject> wave4 = new List<GameObject>();
    [SerializeField] private List<GameObject> wave5 = new List<GameObject>();
    [SerializeField] private List<GameObject> wave6 = new List<GameObject>();
    [SerializeField] private List<Transform> enemySpawnPoints = new List<Transform>();
    private int spawnPointIndex = 0;
    [SerializeField] private GameObject fadeObject;
    [SerializeField] private float fadeStartDelay = 3;
    [SerializeField] private string sceneToLoad = "ShowcaseWin";
    [SerializeField] private float sceneLoadDelay = 5;

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
        if (livingEnemies.Count > 0) return;
        NextWave();
    }

    private void CleanList()
    {
        List<GameObject> newList = new List<GameObject>();

        foreach (GameObject g in livingEnemies)
            if (g != null) newList.Add(g);
        livingEnemies = newList;
    }

    private void NextWave()
    {
        List<GameObject> wave = new List<GameObject>();
        waveNumber++;
        switch (waveNumber)
        {
            case 0:
                wave = wave1;
                break;
            case 1:
                wave = wave2;
                break;
            case 2:
                wave = wave3;
                break;
            case 3:
                wave = wave4;
                break;
            case 4:
                wave = wave5;
                break;
            case 5:
                wave = wave6;
                break;
            case 6:
                StartCoroutine("FadeStartDelay");
                StartCoroutine("SceneChangeDelay");
                break;
        }
        foreach (GameObject g in wave)
        {
            spawnPointIndex++;
            if (spawnPointIndex >= enemySpawnPoints.Count) spawnPointIndex = 0;
            livingEnemies.Add(Instantiate(g, enemySpawnPoints[spawnPointIndex].position, Quaternion.identity));
        }
    }

    private IEnumerator FadeStartDelay()
    {
        yield return new WaitForSeconds(fadeStartDelay);
        fadeObject.SetActive(true);
    }

    private IEnumerator SceneChangeDelay()
    {
        yield return new WaitForSeconds(sceneLoadDelay);
        SceneManager.LoadScene(sceneToLoad);
    }
}
