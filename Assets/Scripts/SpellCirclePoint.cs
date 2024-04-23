using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCirclePoint : MonoBehaviour
{
    [SerializeField] private SpellCircle spellCircle;
    [SerializeField] private SpellCircleLocation circleLocation;
    [SerializeField] private Image filledCircle;
    [SerializeField] private bool setElement = false;
    [SerializeField] private Element elementToSet = Element.none;
    [SerializeField] private bool ignoreTouchIfElementIsNone = false;
    [SerializeField] private bool ignoreTouchIfElementExists = false;

    private void Awake()
    {
        filledCircle.enabled = false;
        spellCircle.AddSpellCirclePoint(this);
        if (circleLocation == SpellCircleLocation.none)
        {
            Debug.LogError("SpellCirclePoint has a SpellCircleLocation of none!");
        }
    }

    /*private void OnTriggerStay(Collider other)
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
    }*/

    public void ResetSpellCirclePoint()
    {
        filledCircle.enabled = false;
    }

    public void TouchedByRay()
    {
        if (ignoreTouchIfElementIsNone && spellCircle.GetElement() == Element.none) return;
        if (ignoreTouchIfElementExists && spellCircle.GetElement() != Element.none) return;
        // if (!filledCircle.enabled) 
        filledCircle.enabled = spellCircle.SpellCirclePointTouched(circleLocation, this) || filledCircle.enabled;
        spellCircle.TouchedByRay();
        if (setElement) spellCircle.SetElement(elementToSet);
    }
}
