using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorHelpers
{
    public static Color SetColorAlpha(Color givenColor, float givenAlpha)
    {
        return new Color(givenColor.r, givenColor.g, givenColor.b, givenAlpha);
    }

    public static Color SetColorRGB(Color givenColor, float r, float g, float b)
    {
        return new Color(r, g, b, givenColor.a);
    }
}
