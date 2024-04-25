using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellbookTab : MonoBehaviour
{
    [SerializeField] private int tabID = 0;
    [SerializeField] private bool relativeTab = false;
    [SerializeField] private int relativeTabChange = 0;
    [SerializeField] private Spellbook spellbook;

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("PlayerHandTabGrabber")) return;
        if (collision.transform.parent.GetComponent<PlayerHand>().GetGripTime() <= 0) return;
        if (!relativeTab) spellbook.TurnToTab(tabID);
        else spellbook.ChangeTab(relativeTabChange);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("PlayerHandTabGrabber")) return;
        if (other.transform.parent.GetComponent<PlayerHand>().GetGripTime() <= 0) return;
        if (!relativeTab) spellbook.TurnToTab(tabID);
        else spellbook.ChangeTab(relativeTabChange);
    }
}
