using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTouchCause : ACause
{
    [SerializeField] private List<string> objectNamesThatCauseTouch = new List<string>();
    [SerializeField] private List<string> tagsThatCauseTouch = new List<string>();
    [SerializeField] private List<string> layersThatCauseTouch = new List<string>();
    [SerializeField] private bool triggerCausesTouch = false;
    [SerializeField] private bool collisionCausesTouch = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!triggerCausesTouch) return;
        if (GameObjectCausesTouch(other.gameObject))
        {
            CauseEffects();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collisionCausesTouch) return;
        if (GameObjectCausesTouch(collision.gameObject))
        {
            CauseEffects();
        }
    }

    private bool GameObjectCausesTouch(GameObject given)
    {
        return tagsThatCauseTouch.Contains(given.tag) 
            || layersThatCauseTouch.Contains(LayerMask.LayerToName(given.layer)) 
            || objectNamesThatCauseTouch.Contains(given.name);
    }
}
