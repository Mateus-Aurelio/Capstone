using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSlider : AHealthTracker
{
    [SerializeField] private Slider healthSlider;

    [SerializeField] private bool hideSliderAtStart;
    [SerializeField] private bool slidersFacePlayer = true;
    private Transform faceTransform;

    private void Start()
    {
        /*if (slidersFacePlayer)
        {
            StartCoroutine();
        }*/
        healthSlider.maxValue = 1;
        healthSlider.value = 1;
        if (hideSliderAtStart) healthSlider.value = 0;

        faceTransform = PlayerTracker.GetPlayer().transform;
        if (slidersFacePlayer && faceTransform != null && faceTransform.GetComponentInChildren<Camera>() != null)
        {
            faceTransform = faceTransform.GetComponentInChildren<Camera>().transform;
        }
    }

    public override void HealthChanged(AHealth healthScript)
    {
        healthSlider.maxValue = healthScript.GetMaxHealth();
        healthSlider.value = healthScript.GetHealth();
    }

    private void LateUpdate()
    {
        if (slidersFacePlayer && healthSlider.value > 0) healthSlider.transform.LookAt(faceTransform.position);
    }
}
