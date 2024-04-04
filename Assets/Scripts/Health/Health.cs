using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : AHealth
{
    // [SerializeField] private float maxHealth = 10;
    // private float health;
    [SerializeField] private List<DamageModifier> damageModifiers = new List<DamageModifier>();
    [SerializeField] private GameObject deathPrefab;
    /*[SerializeField] private Slider hpSlider;
    [SerializeField] private Slider damageSlider;
    //[SerializeField] private int goldToPlayers;
    [SerializeField] private bool hideSliderAtStart;
    [SerializeField] private bool slidersFacePlayer = true;
    private Transform faceTransform;*/

    /*void Start()
    {
        // health = maxHealth;
        if (hideSliderAtStart) hpSlider.gameObject.SetActive(false);
        if (hideSliderAtStart) damageSlider.gameObject.SetActive(false);
        if (slidersFacePlayer)
        {
            faceTransform = PlayerTracker.GetPlayer().transform;
            if (faceTransform != null && faceTransform.GetComponentInChildren<Camera>() != null)
            {
                faceTransform = faceTransform.GetComponentInChildren<Camera>().transform;
            }
        }
    }*/

    /*void LateUpdate()
    {
        UpdateSlider();
        DeathCheck();
    }*/

    /*public float GetHealth() { return health; }
    public float GetMaxHealth() { return maxHealth; }
    public Slider GetHealthSlider() { return hpSlider; }
    public Slider GetDamageSlider() { return damageSlider; }*/

    /*private void UpdateSlider()
    {
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHealth;
            hpSlider.value = health;
        }
        if (damageSlider != null)
        {
            damageSlider.maxValue = maxHealth;
            damageSlider.value = Mathf.Lerp(damageSlider.value, health, Time.deltaTime * 6f);
        }
        if (hpSlider != null && faceTransform != null) hpSlider.transform.LookAt(faceTransform.position);
        if (damageSlider != null && faceTransform != null) damageSlider.transform.LookAt(faceTransform.position);
    }*/

    public override void Damage(float damage, DamageType damageType = DamageType.none)
    {
        float modifier = 1;
        foreach (DamageModifier dm in damageModifiers)
        {
            if (dm.damageType == damageType)
            {
                modifier *= dm.modifier;
                break;
            }
        }
        damage *= modifier;
        /*if (hpSlider != null && !hpSlider.gameObject.activeInHierarchy) hpSlider.gameObject.SetActive(true);
        if (damageSlider != null && !damageSlider.gameObject.activeInHierarchy) damageSlider.gameObject.SetActive(true);*/
        ChangeHealth(-damage);
    }

    /*public override void Heal(float given)
    {
        // health = Mathf.Clamp(health + given, 0, 100000);
        ChangeHealth(given);
    }*/

    /*private void DeathCheck()
    {
        if (health <= 0) Die();
    }*/

    public override void Die()
    {
        if (deathPrefab != null) Instantiate(deathPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
