using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerTracker
{
    public static GameObject player;

    public static GameObject GetPlayer()
    {
        if (player == null)
        {
            FindPlayer();
        }
        if (player == null)
        {
            Debug.LogWarning("PlayerTracker unable to find player.");
        }
        return player;
    }

    private static void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
