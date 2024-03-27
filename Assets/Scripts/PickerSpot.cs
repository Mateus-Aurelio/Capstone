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

    private void Awake()
    {
        startColor = GetComponent<Image>().color;
        if (startColor.a <= 0) startColor = Color.gray;
    }

    public void HandTouched()
    {
        GetComponent<Image>().color = Color.white;
        if (subSpots != null && subSpots.Count > 0)
        {
            RevealSubSpots();
            if (hideSelfWhenReveal) HideSpot();
        }
    }

    public void HandNotTouched()
    {
        GetComponent<Image>().color = startColor;
    }

    public void HandReleased(PlayerHand hand)
    {
        if (spellPrefab == null)
            return;
        GameObject spell = Instantiate(spellPrefab, transform.position, spellPrefab.transform.rotation);
        spell.GetComponent<Orb>().SetHandObject(hand);
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
