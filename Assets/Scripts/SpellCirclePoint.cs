using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCirclePoint : MonoBehaviour
{
    [SerializeField] private SpellCircle spellCircle;
    [SerializeField] private SpellCircleLocation circleLocation;

    private void OnTriggerEnter(Collider other)
    {
        CollidedObjectCheck(other.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CollidedObjectCheck(collision.gameObject);
    }

    private void CollidedObjectCheck(GameObject obj)
    {
        if (!obj.CompareTag("PlayerHand")) return;
        PlayerHand hand = obj.GetComponent<PlayerHand>();
        if (hand == null) return;
        if (spellCircle.GetCastingHand() != null && hand != spellCircle.GetCastingHand()) return;
        if (spellCircle.GetCastingHand() == null) spellCircle.SetCastingHand(hand);
        if (!hand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger)) return;
        spellCircle.SpellCirclePointTouched(circleLocation);
    }
}
