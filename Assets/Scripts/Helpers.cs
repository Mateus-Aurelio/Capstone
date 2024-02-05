using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static Color SetAlpha(Color given, float newAlpha)
    {
        return new Color(given.r, given.g, given.b, newAlpha);
    }

    public static Color ChangeAlpha(Color given, float alphaChange)
    {
        return new Color(given.r, given.g, given.b, given.a + alphaChange);
    }
}
