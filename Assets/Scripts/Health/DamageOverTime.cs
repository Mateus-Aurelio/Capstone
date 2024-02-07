using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    [SerializeField] private float damagePerSecond = 2;
    [SerializeField] private float maxTriggersPerSecond = 1;

    [SerializeField] private bool damageEnemies = true;
    [SerializeField] private bool damagePlayers = false;
    [SerializeField] private bool damageOnTrigger;
    [SerializeField] private bool damageOnCollision;

    // private List<GameObject> gameObjectsInside = new List<GameObject>();
    private List<(GameObject, float)> gameObjectsInside = new List<(GameObject, float)>();


    private void OnCollisionEnter(Collision collision)
    {
        if (!damageOnCollision) return;
        /*if (damageEnemies && (collision.gameObject.GetComponent<Enemy>() != null || collision.gameObject.CompareTag("Enemy")))
        {
            if (collision.gameObject.GetComponent<AHealth>() != null) gameObjectsInside.Add((collision.gameObject, 0));
        }

        if (damagePlayers && (collision.gameObject.GetComponent<PlayerMove>() != null || collision.gameObject.CompareTag("Player")))
        {
            if (collision.gameObject.GetComponent<AHealth>() != null) gameObjectsInside.Add((collision.gameObject, 0));
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!damageOnTrigger) return;
        /*if (damageEnemies && (other.gameObject.GetComponent<Enemy>() != null || other.CompareTag("Enemy")))
        {
            if (other.gameObject.GetComponent<AHealth>() != null) gameObjectsInside.Add((other.gameObject, 0));
        }

        if (damagePlayers && (other.gameObject.GetComponent<PlayerMove>() != null || other.CompareTag("Player")))
        {
            if (other.gameObject.GetComponent<AHealth>() != null) gameObjectsInside.Add((other.gameObject, 0));
        }*/
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!damageOnCollision) return;
        foreach ((GameObject, float) pair in gameObjectsInside)
        {
            if (pair.Item1 == collision.gameObject)
            {
                gameObjectsInside.Remove((pair.Item1, pair.Item2));
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!damageOnTrigger) return;
        foreach ((GameObject, float) pair in gameObjectsInside)
        {
            if (pair.Item1 == other.gameObject)
            {
                gameObjectsInside.Remove((pair.Item1, pair.Item2));
                return;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!damageOnCollision) return;
        foreach ((GameObject, float) pair in gameObjectsInside)
        {
            if (pair.Item1 == collision.gameObject)
            {
                if (pair.Item2 >= 1 / maxTriggersPerSecond)
                {
                    pair.Item1.GetComponent<AHealth>().Damage(damagePerSecond / maxTriggersPerSecond);
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

    private void OnTriggerStay(Collider other)
    {
        if (!damageOnTrigger) return;
        foreach ((GameObject, float) pair in gameObjectsInside)
        {
            if (pair.Item1 == other.gameObject)
            {
                if (pair.Item2 >= 1 / maxTriggersPerSecond)
                {
                    pair.Item1.GetComponent<AHealth>().Damage(damagePerSecond / maxTriggersPerSecond);
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

    /*public void ChangeDamage(float change)
    {
        damage += change;
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }*/
}
