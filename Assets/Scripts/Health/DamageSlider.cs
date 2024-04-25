using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageSlider : AHealthTracker
{
    private AHealth health;
    [SerializeField] Slider damageSlider;
    [SerializeField] float speed = 6;
    [SerializeField] private bool hideSliderAtStart = true;
    [SerializeField] private GameObject background;
    [SerializeField] private bool slidersFacePlayer = true;
    private Transform faceTransform;

    private void Start()
    {
        damageSlider.maxValue = 1;
        damageSlider.value = 1;
        if (hideSliderAtStart) 
        {
            damageSlider.value = 0;
            background.SetActive(false);
        }

        faceTransform = PlayerTracker.GetPlayer().transform;
        if (slidersFacePlayer && faceTransform != null && faceTransform.GetComponentInChildren<Camera>() != null)
        {
            faceTransform = faceTransform.GetComponentInChildren<Camera>().transform;
        }
    }

    public override void HealthChanged(AHealth healthScript)
    {
        health = healthScript;
        if (hideSliderAtStart && damageSlider.value == 0)
        {
            damageSlider.value = 1;
            background.SetActive(true);
            hideSliderAtStart = false;
        }
        StopCoroutine("UpdateSlider");
        StartCoroutine("UpdateSlider");
    }

    private IEnumerator UpdateSlider()
    {
        while (damageSlider.value != health.GetHealthRatio())
        {
            damageSlider.value = Mathf.Lerp(damageSlider.value, health.GetHealthRatio(), Time.deltaTime * speed);
            yield return new WaitForEndOfFrame();
        }
    }

    private void LateUpdate()
    {
        if (slidersFacePlayer && damageSlider.value > 0) damageSlider.transform.LookAt(faceTransform.position);
    }
}
