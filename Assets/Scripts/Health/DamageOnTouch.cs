using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    /*[SerializeField] private float damage;
    [SerializeField] private DamageType damageType;*/
    [SerializeField] Damage damage;

    [SerializeField] private bool damageEnemies = true;
    [SerializeField] private bool damagePlayers = false;
    [SerializeField] private bool damageTrees = false;
    [SerializeField] private bool damageOnTrigger;
    [SerializeField] private bool damageOnCollision;
    [SerializeField] private bool destroySelfOnDamage = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!damageOnCollision) return;
        TryToDamage(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!damageOnTrigger) return;
        TryToDamage(other.gameObject);
    }

    private void TryToDamage(GameObject obj)
    {
        if (obj == null) return;
        AHealth healthScript = obj.GetComponent<AHealth>();
        if (healthScript == null) return;

        bool destroy = false;
        if (damageEnemies && (obj != null && obj.CompareTag("Enemy")))
        {
            damage.DealDamage(healthScript);
            if (!destroy && destroySelfOnDamage) destroy = true;
        }

        if (damagePlayers && obj.CompareTag("PlayerHurtbox"))
        {
            damage.DealDamage(healthScript);
            if (!destroy && destroySelfOnDamage) destroy = true;
        }

        if (damageTrees && obj.CompareTag("Tree"))
        {
            damage.DealDamage(healthScript);
            if (!destroy && destroySelfOnDamage) destroy = true;
        }

        if (destroy) Destroy(gameObject);
    }
}
