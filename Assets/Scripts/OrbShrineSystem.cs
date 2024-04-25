using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrbShrineSystem : MonoBehaviour
{
    [SerializeField] private int shrinesToActivate = 8;
    private int shrinesActivated = 0;
    [SerializeField] private GameObject activatedObject;
    [SerializeField] private GameObject fadeObject;
    [SerializeField] private float fadeStartDelay = 5;
    [SerializeField] private string sceneToLoad = "ShowcaseMain";
    [SerializeField] private float sceneLoadDelay = 8;

    private void Start()
    {
        shrinesActivated = 0;
    }

    public void ActivatedOrbShrine()
    {
        Debug.Log("ActivatedOrbShrine");
        shrinesActivated++;
        Debug.Log("shrinesActivated: " + shrinesActivated);
        if (shrinesActivated >= shrinesToActivate)
        {
            Debug.Log("reached shrinesToActivate: " + shrinesToActivate);
            shrinesToActivate = 99;
            activatedObject.SetActive(true);
            StartCoroutine("FadeStartDelay");
            StartCoroutine("SceneChangeDelay");
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
