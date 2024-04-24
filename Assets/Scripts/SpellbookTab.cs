using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellbookTab : MonoBehaviour
{
    [SerializeField] private int tabID = 0;
    [SerializeField] private Spellbook spellbook;

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("PlayerHandTabGrabber")) return;
        spellbook.TurnToTab(tabID);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("PlayerHandTabGrabber")) return;
        spellbook.TurnToTab(tabID);
    }
}
