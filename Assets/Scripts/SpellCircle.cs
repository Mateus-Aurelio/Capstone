using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCircle : MonoBehaviour
{
    private SpellCircleLocation lastLocation = SpellCircleLocation.none;
    private List<SpellCircleEdge> edges = new List<SpellCircleEdge>();
    [SerializeField] private List<GameObject> spells = new List<GameObject>();
    [SerializeField] private List<PlayerHand> hands = new List<PlayerHand>();
    private PlayerHand castingHand = null;

    private void Update()
    {
        foreach (PlayerHand hand in hands)
        {
            // if (hand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Grip))
        }

        if (castingHand == null) return; 

        if (!castingHand.HandButtonPressed(UnityEngine.XR.Interaction.Toolkit.InputHelpers.Button.Trigger))
        {
            CastAttempt();
        }
    }

    public void SpellCirclePointTouched(SpellCircleLocation circleLocation)
    {
        if (lastLocation == circleLocation || circleLocation == SpellCircleLocation.none) return;

        if (lastLocation != SpellCircleLocation.none)
        {
            edges.Add(new SpellCircleEdge(lastLocation, circleLocation));
        }

        lastLocation = circleLocation;
    }

    public void CastAttempt()
    {
        if (edges.Count <= 1)
        {
            lastLocation = SpellCircleLocation.none;
            edges.Clear();
            castingHand = null;
            return;
        }

        foreach (GameObject prefab in spells)
        {
            if (prefab.GetComponent<Spell>() == null) return;
            
            foreach (SpellEdges edgeToCast in prefab.GetComponent<Spell>().GetSpellEdges())
            {
                foreach (SpellCircleEdge spellEdgeInEdges in edges)
                {
                    if (!edgeToCast.spellCircleEdges.Contains(spellEdgeInEdges))
                    {
                        return;
                    }
                }
            }
        }

        lastLocation = SpellCircleLocation.none;
        edges.Clear();
        castingHand = null;
    }

    public PlayerHand GetCastingHand()
    {
        return castingHand;
    }

    public void SetCastingHand(PlayerHand given)
    {
        castingHand = given;
    }

}

public enum SpellCircleLocation
{
    none = 0,
    top = 1,
    topRight = 2,
    right = 3,
    bottomRight = 4, 
    bottom = 5,
    bottomLeft = 6, 
    left = 7, 
    topLeft = 8
}

[Serializable]
public struct SpellCircleEdge
{
    public SpellCircleEdge(SpellCircleLocation given1, SpellCircleLocation given2)
    {
        location1 = given1;
        location2 = given2;
    }

    public SpellCircleLocation location1;
    public SpellCircleLocation location2;
}
