using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickerSpot : MonoBehaviour
{
    [SerializeField] private List<PickerSpot> subSpots = new List<PickerSpot>();
    [SerializeField] private GameObject spellPrefab;
    private Color startColor;
    [SerializeField] private bool hideSelfWhenReveal = false;
    [SerializeField] private string displayName = "";

    private void Awake()
    {
        startColor = GetComponent<Image>().color;
        if (startColor.a <= 0) startColor = Color.gray;
    }

    public void HandTouched(PlayerHand hand, SpellPicker spellPicker)
    {
        GetComponent<Image>().color = new Color(1, 1, 1, startColor.a);
        if (subSpots != null && subSpots.Count > 0)
        {
            RevealSubSpots();
            if (hideSelfWhenReveal) HideSpot();
        }
    }

    public void HandNotTouched(SpellPicker spellPicker)
    {
        GetComponent<Image>().color = startColor;
    }

    public void HandReleased(PlayerHand hand, SpellPicker spellPicker)
    {
        if (spellPrefab == null)
            return;
        GameObject spell = Instantiate(spellPrefab, spellPicker.transform.position, spellPrefab.transform.rotation);
        Orb orb = spell.GetComponent<Orb>();
        if (orb != null) orb.SetHandObject(hand);
    }

    public void RevealSpot()
    {
        gameObject.SetActive(true); // temp
    }

    public void HideSpot()
    {
        GetComponent<Image>().color = startColor;
        gameObject.SetActive(false); // temp
    }

    public void RevealSubSpots()
    {
        if (subSpots == null || subSpots.Count <= 0)
            return;
        foreach (PickerSpot spot in subSpots)
        {
            spot.RevealSpot();
        }
    }

    public void HideSubSpots()
    {
        if (subSpots == null || subSpots.Count <= 0)
            return;
        foreach (PickerSpot spot in subSpots)
        {
            spot.HideSpot();
        }
    }

    public List<PickerSpot> GetSubSpots()
    {
        return subSpots;
    }
}
