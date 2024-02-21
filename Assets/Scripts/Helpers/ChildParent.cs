using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildParent : MonoBehaviour
{
    [SerializeField] private bool normalCollisions = true;
    [SerializeField] private bool triggerCollisions = false;
    [SerializeField] private List<string> tagsToCollect = new List<string>();
    [SerializeField] private List<string> layersToCollect = new List<string>();
    private List<Transform> collectedChildren = new List<Transform>();
    [SerializeField] private bool grabTruePlayerParentTagInsteadOfPlayerGrounderTag = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (normalCollisions) TryToCollect(collision.gameObject.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerCollisions) TryToCollect(other.gameObject.transform);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (normalCollisions) TryToCollect(collision.gameObject.transform);
    }

    private void OnTriggerStay(Collider other)
    {
        if (triggerCollisions) TryToCollect(other.gameObject.transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (normalCollisions) TryToUncollect(collision.gameObject.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerCollisions) TryToUncollect(other.gameObject.transform);
    }

    private void TryToCollect(Transform given)
    {
        if (grabTruePlayerParentTagInsteadOfPlayerGrounderTag && given.CompareTag("PlayerGrounder"))
        {
            given = GameObject.FindGameObjectWithTag("TruePlayerParent").transform;
            given.SetParent(this.gameObject.transform);
            collectedChildren.Add(given);
            Debug.Log("Collected TruePlayerParent");
            return;
        }
        Debug.Log("Trying to collect " + given.name);
        if (collectedChildren.Contains(given)) return;
        foreach (string tag in tagsToCollect)
        {
            if (given.CompareTag(tag))
            {
                Debug.Log("collected! " + given.name);
                if (grabTruePlayerParentTagInsteadOfPlayerGrounderTag && tag == "PlayerGrounder")
                {
                    given = GameObject.FindGameObjectWithTag("TruePlayerParent").transform; 
                }
                given.SetParent(this.gameObject.transform);
                collectedChildren.Add(given);
                return;
            }
        }
        foreach (string layer in layersToCollect)
        {
            if (given.gameObject.layer == LayerMask.NameToLayer(layer))
            {
                Debug.Log("collected! " + given.name);
                given.SetParent(this.gameObject.transform);
                collectedChildren.Add(given);
                return;
            }
        }
        Debug.Log("NOT in layers nor tags " + given.name);
    }

    private void TryToUncollect(Transform given)
    {
        if (grabTruePlayerParentTagInsteadOfPlayerGrounderTag && given.CompareTag("PlayerGrounder"))
        {
            given = GameObject.FindGameObjectWithTag("TruePlayerParent").transform;
            given.SetParent(null);
            collectedChildren.Remove(given);
            Debug.Log("Uncollected TruePlayerParent");
            return;
        }

        if (!collectedChildren.Contains(given) && !grabTruePlayerParentTagInsteadOfPlayerGrounderTag) return;
        Debug.Log("Trying to uncollect " + given.name);

        foreach (string tag in tagsToCollect)
        {
            if (given.gameObject.CompareTag(tag))
            {
                Debug.Log("uncollected! " + given.name);
                given.SetParent(null);
                collectedChildren.Remove(given);
                return;
            }
        }
        foreach (string layer in layersToCollect)
        {
            if (given.gameObject.layer == LayerMask.NameToLayer(layer))
            {
                Debug.Log("uncollected! " + given.name);
                given.SetParent(null);
                collectedChildren.Remove(given);
                return;
            }
        }
        Debug.Log("NOT in layers nor tags " + given.name);
    }

    private void OnDestroy()
    {
        foreach (Transform t in collectedChildren)
        {
            t.SetParent(null);
        }
    }
}

