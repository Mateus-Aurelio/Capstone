using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnExitCause : ACause
{
    [SerializeField] private List<string> objectNamesThatCauseExit = new List<string>();
    [SerializeField] private List<string> tagsThatCauseExit = new List<string>();
    [SerializeField] private List<string> layersThatCauseExit = new List<string>();
    [SerializeField] private bool triggerCausesExit = false;
    [SerializeField] private bool collisionCausesExit = false;

    private void OnTriggerExit(Collider other)
    {
        if (!triggerCausesExit) return;
        if (GameObjectCausesExit(other.gameObject))
        {
            CauseEffects();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collisionCausesExit) return;
        if (GameObjectCausesExit(collision.gameObject))
        {
            CauseEffects();
        }
    }

    private bool GameObjectCausesExit(GameObject given)
    {
        return tagsThatCauseExit.Contains(given.tag)
            || layersThatCauseExit.Contains(LayerMask.LayerToName(given.layer))
            || objectNamesThatCauseExit.Contains(given.name);
    }
}
