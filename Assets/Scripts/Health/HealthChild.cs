using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthChild : AHealth
{
    [SerializeField] private float damageModifer = 2;
    [SerializeField] private Health healthScript;

    void Start()
    {
        //health = maxHealth;
        //if (hideSliderAtStart) hpSlider.gameObject.SetActive(false);
        //if (hideSliderAtStart) damageSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        //UpdateSlider();
        //DeathCheck();
    }

    //public float GetHealth() { return health; }
    //public float GetMaxHealth() { return maxHealth; }
    //public Slider GetHealthSlider() { return hpSlider; }
    //public Slider GetDamageSlider() { return damageSlider; }

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
    }*/

    public override void Damage(float given)
    {
        healthScript.Damage(given * damageModifer);
        //if (!hpSlider.gameObject.activeInHierarchy) hpSlider.gameObject.SetActive(true);
        //if (!damageSlider.gameObject.activeInHierarchy) damageSlider.gameObject.SetActive(true);
        //health = Mathf.Clamp(health - given, 0, 100000);
    }

    public override void Heal(float given)
    {
        healthScript.Heal(given);
    }

    /*private void DeathCheck()
    {
        if (health <= 0) Die();
    }*/

    /*public void Die()
    {
        health = 0;
        Destroy(gameObject);
    }*/
}
