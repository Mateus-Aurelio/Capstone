using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private float damage;

    [SerializeField] private bool damageEnemies = true;
    [SerializeField] private bool damagePlayers = false;
    [SerializeField] private bool damageOnTrigger;
    [SerializeField] private bool damageOnCollision;
    [SerializeField] private bool destroySelfOnDamage = false;

    [SerializeField] private float delayedDestroy = -1;
    [SerializeField] private List<Component> delayedDestroyComponentList = new List<Component>();
    [SerializeField] private List<GameObject> delayedDestroyGameObjectList = new List<GameObject>();

    private void Start()
    {
        if (delayedDestroy >= 0)
        {
            foreach (Component c in delayedDestroyComponentList) Destroy(c, delayedDestroy);
            foreach (GameObject g in delayedDestroyGameObjectList) Destroy(g, delayedDestroy);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!damageOnCollision) return;
        bool destroy = false;
        if (damageEnemies && (collision.gameObject.GetComponent<Enemy>() != null || collision.gameObject.CompareTag("Enemy")))
        {
            AHealth health = collision.gameObject.GetComponent<AHealth>();
            if (health != null) health.Damage(damage);
            if (!destroy && destroySelfOnDamage) destroy = true;
        }

        if (damagePlayers && (collision.gameObject.GetComponent<PlayerMove>() != null || collision.gameObject.CompareTag("Player")))
        {
            AHealth health = collision.gameObject.GetComponent<AHealth>();
            if (health != null) health.Damage(damage);
            if (!destroy && destroySelfOnDamage) destroy = true;
        }

        if (destroy) Destroy(gameObject); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!damageOnTrigger) return;
        bool destroy = false;
        if (damageEnemies && (other.gameObject.GetComponent<Enemy>() != null || other.CompareTag("Enemy")))
        {
            AHealth health = other.gameObject.GetComponent<AHealth>();
            if (health != null) health.Damage(damage);
            if (!destroy && destroySelfOnDamage) destroy = true;
        }

        if (damagePlayers && (other.gameObject.GetComponent<PlayerMove>() != null || other.CompareTag("Player")))
        {
            AHealth health = other.gameObject.GetComponent<AHealth>();
            if (health != null) health.Damage(damage);
            if (!destroy && destroySelfOnDamage) destroy = true;
        }

        if (destroy) Destroy(gameObject);
    }

    public void ChangeDamage(float change)
    {
        damage += change;
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
}
