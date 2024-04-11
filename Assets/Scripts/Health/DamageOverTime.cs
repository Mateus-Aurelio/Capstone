using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    /*[SerializeField] private DamageType damageType;
    [SerializeField] private float damagePerSecond = 2;*/
    [SerializeField] private Damage damage;
    [SerializeField] private float maxTriggersPerSecond = 1;

    [SerializeField] private bool damageEnemies = true;
    [SerializeField] private bool damagePlayers = false;
    [SerializeField] private bool damageOnTrigger;
    [SerializeField] private bool damageOnCollision;

    private List<(GameObject, float)> gameObjectsInside = new List<(GameObject, float)>();


    private void OnCollisionEnter(Collision collision)
    {
        if (!damageOnCollision) return;
        AttemptToAddObject(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!damageOnTrigger) return;
        AttemptToAddObject(other.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!damageOnCollision) return;
        AttemptToRemoveObject(collision.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!damageOnTrigger) return;
        AttemptToRemoveObject(other.gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!damageOnCollision) return;
        AttemptToDamageObject(collision.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!damageOnTrigger) return;
        AttemptToDamageObject(other.gameObject);
    }

    private void AttemptToAddObject(GameObject obj)
    {
        if (damageEnemies && (obj/*.GetComponent<Enemy>()*/ != null && obj.CompareTag("Enemy")))
        {
            if (obj.GetComponent<AHealth>() != null) gameObjectsInside.Add((obj, -1));
        }

        if (damagePlayers && (obj/*.GetComponent<PlayerMove>()*/ != null && obj.CompareTag("Player")))
        {
            if (obj.GetComponent<AHealth>() != null) gameObjectsInside.Add((obj, -1));
        }
    }

    private void AttemptToRemoveObject(GameObject obj)
    {
        foreach ((GameObject, float) pair in gameObjectsInside)
        {
            if (pair.Item1 == obj)
            {
                gameObjectsInside.Remove((pair.Item1, pair.Item2));
                return;
            }
        }
    }

    private void AttemptToDamageObject(GameObject obj)
    {
        foreach ((GameObject, float) pair in gameObjectsInside)
        {
            if (pair.Item1 == obj)
            {
                if (pair.Item2 == -1 || pair.Item2 >= 1 / maxTriggersPerSecond)
                {
                    // pair.Item1.GetComponent<AHealth>().Damage(damagePerSecond / maxTriggersPerSecond, damageType);
                    damage.DealDamage(pair.Item1.GetComponent<AHealth>());
                    // pair.Item1.GetComponent<AHealth>().Damage(damagePerSecond / maxTriggersPerSecond, damageType);
                    gameObjectsInside.Add((pair.Item1, 0));
                    gameObjectsInside.Remove((pair.Item1, pair.Item2));
                    return;
                }
                gameObjectsInside.Add((pair.Item1, pair.Item2 + Time.deltaTime));
                gameObjectsInside.Remove((pair.Item1, pair.Item2));
                return;
            }
        }
    }
}
