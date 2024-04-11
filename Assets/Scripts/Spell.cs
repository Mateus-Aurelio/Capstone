using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private Element elementToCast;
    [SerializeField] private List<SpellEdgesOption> spellEdgesOptions = new List<SpellEdgesOption>();

    public List<SpellEdgesOption> GetSpellEdgesOptions()
    {
        return spellEdgesOptions;
    }

    public virtual void SpellInit(PlayerHand mainHand)
    {

    }

    public Element GetElementToCast()
    {
        return elementToCast;
    }
}

[Serializable]
public struct SpellEdgesOption
{
    public List<SpellCircleEdge> spellCircleEdges;

    public SpellEdgesOption(List<SpellCircleEdge> given)
    {
        spellCircleEdges = given;
    }
}