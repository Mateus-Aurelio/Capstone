using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbShrine : MonoBehaviour
{
    [SerializeField] Element element = Element.fire;
    [SerializeField] GameObject activatedGameObject;
    [SerializeField] OrbShrineSystem orbShrineSystem;
    private bool active;
    [SerializeField] private bool activateOnStart = false;

    private void Start()
    {
        if (activateOnStart) StartCoroutine("DelayedActivate");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Fire"))
        {
            Activate();
            return;
        }
        if (other.GetComponent<Orb>() == null) return;
        if (other.GetComponent<Orb>().GetElementToCast() == element) Activate();
    }

    private void Activate()
    {
        active = true;
        activatedGameObject.SetActive(true);
        orbShrineSystem.ActivatedOrbShrine();
    }

    private IEnumerator DelayedActivate()
    {
        yield return new WaitForSeconds(0.5f);
        Activate();
    }
}
