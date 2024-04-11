using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCirclePoint : MonoBehaviour
{
    [SerializeField] private SpellCircle spellCircle;
    [SerializeField] private SpellCircleLocation circleLocation;
    [SerializeField] private Image filledCircle;

    private void Awake()
    {
        filledCircle.enabled = false;
        spellCircle.AddSpellCirclePoint(this);
    }

    private void OnTriggerStay(Collider other)
    {
        CollidedObjectCheck(other.gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        CollidedObjectCheck(collision.gameObject);
    }

    private void CollidedObjectCheck(GameObject obj)
    {
        if (!obj.CompareTag("PlayerHand")) return;
        PlayerHand hand = obj.GetComponent<PlayerHand>();
        if (hand == null) return;
        if (hand.GetHand() == Hand.left) return;
        if (!hand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger)) return;
        if (spellCircle.GetPreparingHand() != null && hand == spellCircle.GetPreparingHand()) return;
        if (spellCircle.GetCastingHand() != null && hand != spellCircle.GetCastingHand()) return;
        if (spellCircle.GetCastingHand() == null) spellCircle.SetCastingHand(hand);
        filledCircle.enabled = spellCircle.SpellCirclePointTouched(circleLocation, this);
    }

    public void ResetSpellCirclePoint()
    {
        filledCircle.enabled = false;
    }
}
