using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSource : ZoneEffect
{
    public static List<ElementSource> sources = new List<ElementSource>();
    [SerializeField] private List<GameObject> gameObjectsWhenShowing = new List<GameObject>();
    [SerializeField] private Element element = Element.none;

    private void Start()
    {
        sources.Add(this);
        HideSource();
    }

    public override void DoEnterEffect(string effectName)
    {
        if (effectName == "ElementSourceFinder") ShowSource();
    }

    public override void DoExitEffect(string effectName)
    {
        if (effectName == "ElementSourceFinder") HideSource();
    }

    public void ShowSource()
    {
        foreach (GameObject g in gameObjectsWhenShowing)
        {
            g.SetActive(true);
        }
    }

    public void HideSource()
    {
        foreach (GameObject g in gameObjectsWhenShowing)
        {
            g.SetActive(false);
        }
    }

    public Element GetElement()
    {
        return element;
    }
}
