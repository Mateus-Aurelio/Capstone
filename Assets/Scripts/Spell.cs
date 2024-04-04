using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField] private Element elementToCast;
    [SerializeField] private List<SpellEdges> edgesToCast = new List<SpellEdges>();

    public List<SpellEdges> GetSpellEdges()
    {
        return edgesToCast;
    }

    public virtual void SpellInit(PlayerHand mainHand)
    {

    }
}

[Serializable]
public struct SpellEdges
{
    public List<SpellCircleEdge> spellCircleEdges;

    public SpellEdges(List<SpellCircleEdge> given)
    {
        spellCircleEdges = given;
    }
}