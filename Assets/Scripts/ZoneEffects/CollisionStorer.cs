using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionStorer : MonoBehaviour
{
    private List<GameObject> objectsColliding = new List<GameObject>();
    [SerializeField] private List<string> tagsThatCollide = new List<string>();
    [SerializeField] private List<string> layersThatCollide = new List<string>();
    [SerializeField] private bool doNormalCollisions = true;
    [SerializeField] private bool doTriggerCollisions = false;
    [SerializeField] private bool requireRaycast = false;
    [SerializeField] private string zoneEffectName = "";
    private Camera raycastCam;

    private void Start()
    {
        if (raycastCam == null && requireRaycast)
        {
            Debug.LogWarning("raycastCam not found in CollisionStorer, setting requireRaycast to FALSE");
            requireRaycast = false;
        }
    }

    private bool RaycastCheck(GameObject givenObject)
    {
        if (!requireRaycast)
        {
            return true;
        }
        Ray ray = new Ray(raycastCam.transform.position, (givenObject.transform.position - raycastCam.transform.position));
        RaycastHit hitData;
        Physics.Raycast(ray, out hitData);
        /*Debug.DrawRay(playerCam.transform.position, (givenObject.transform.position - playerCam.transform.position));
        Debug.Log("Raycast to object " + givenObject.name);
        Debug.Log("Raycast to position " + givenObject.transform.position);
        Debug.Log("Raycast from position " + playerCam.transform.position);
        Debug.Log("Raycast point " + hitData.point);
        Debug.Log("Raycast distance " + hitData.distance);*/
        if (hitData.transform == null)
        {
            Debug.Log("Raycast found nothing");
            return false;
        }
        Debug.Log("Raycast found " + hitData.transform.gameObject.name);
        return hitData.transform.gameObject == givenObject;
    }

    private void TryToEnter(GameObject givenObject)
    {
        if (objectsColliding.Contains(givenObject))
        {
            return;
        }
        if (!RaycastCheck(givenObject))
        {
            return;
        }
        objectsColliding.Add(givenObject);
        ZoneEffect[] zoneEffects = givenObject.GetComponents<ZoneEffect>();
        foreach (ZoneEffect ze in zoneEffects)
        {
            ze.DoEnterEffect(zoneEffectName);
        }
        Debug.Log("entered " + givenObject.name);
    }

    private void TryToExit(GameObject givenObject)
    {
        if (!objectsColliding.Contains(givenObject))
        {
            return;
        }
        if (!RaycastCheck(givenObject))
        {
            return;
        }
        objectsColliding.Remove(givenObject);
        ZoneEffect[] zoneEffects = givenObject.GetComponents<ZoneEffect>();
        foreach (ZoneEffect ze in zoneEffects)
        {
            ze.DoExitEffect(zoneEffectName);
        }
        Debug.Log("exited " + givenObject.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!doNormalCollisions) return;
        if (GameObjectCausesTouch(collision.gameObject))
        {
            if (collision.gameObject.CompareTag(tag) && !objectsColliding.Contains(collision.gameObject))
            {
                TryToEnter(collision.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (!doTriggerCollisions) return;
        if (GameObjectCausesTouch(other.gameObject))
        {
            if (other.CompareTag(tag) && !objectsColliding.Contains(other.gameObject))
            {
                TryToEnter(other.gameObject);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!doNormalCollisions) return;
        if (GameObjectCausesTouch(collision.gameObject))
        {
            if (collision.gameObject.CompareTag(tag))
            {
                /*Ray ray = new Ray(transform.position, transform.position - collision.transform.position);
                RaycastHit hitData;
                Physics.Raycast(ray, out hitData);
                if (hitData.transform == null)
                {
                    return;
                }
                GameObject hitObject = hitData.transform.gameObject;*/
                if (!objectsColliding.Contains(collision.gameObject))// && hitObject == collision.gameObject)
                {
                    TryToEnter(collision.gameObject);
                }
                else if (objectsColliding.Contains(collision.gameObject) && requireRaycast && !RaycastCheck(collision.gameObject))
                {
                    TryToExit(collision.gameObject);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!doTriggerCollisions) return;
        if (GameObjectCausesTouch(other.gameObject))
        {
            if (other.CompareTag(tag))
            {
                /*Ray ray = new Ray(transform.position, transform.position - other.transform.position);
                RaycastHit hitData;
                Physics.Raycast(ray, out hitData);
                if (hitData.transform == null)
                {
                    return;
                }
                GameObject hitObject = hitData.transform.gameObject;*/
                if (!objectsColliding.Contains(other.gameObject))// && hitObject == other.gameObject)
                {
                    TryToEnter(other.gameObject);
                }
                else if(objectsColliding.Contains(other.gameObject) && requireRaycast && !RaycastCheck(other.gameObject))
                {
                    TryToExit(other.gameObject);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!doNormalCollisions) return;
        if (GameObjectCausesTouch(collision.gameObject))
        {
            if (collision.gameObject.CompareTag(tag) && objectsColliding.Contains(collision.gameObject))
            {
                TryToExit(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!doTriggerCollisions) return;
        if (GameObjectCausesTouch(other.gameObject))
        {
            if (other.CompareTag(tag) && objectsColliding.Contains(other.gameObject))
            {
                TryToExit(other.gameObject);
            }
        }
    }

    private bool GameObjectCausesTouch(GameObject given)
    {
        return tagsThatCollide.Contains(given.tag)
            || layersThatCollide.Contains(LayerMask.LayerToName(given.layer));
    }

    public List<GameObject> GetObjectsColliding()
    {
        return objectsColliding;
    }
}
