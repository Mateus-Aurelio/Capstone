using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTouchCause : ACause
{
    [SerializeField] private List<string> objectNamesThatCauseTouch = new List<string>();
    [SerializeField] private bool triggerCausesTouch = false;
    [SerializeField] private bool collisionCausesTouch = false;
    [SerializeField] private bool twoDimensional = false;

    private void OnTriggerEnter(Collider other)
    {
        if (twoDimensional) return;
        if (!triggerCausesTouch) return;
        if (objectNamesThatCauseTouch.Contains(other.name))
        {
            CauseEffects();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (twoDimensional) return;
        if (!collisionCausesTouch) return;
        if (objectNamesThatCauseTouch.Contains(collision.gameObject.name))
        {
            CauseEffects();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!twoDimensional) return;
        if (!triggerCausesTouch) return;
        if (objectNamesThatCauseTouch.Contains(collision.gameObject.name))
        {
            CauseEffects();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!twoDimensional) return;
        if (!collisionCausesTouch) return;
        if (objectNamesThatCauseTouch.Contains(collision.gameObject.name))
        {
            CauseEffects();
        }
    }
}
