using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 0.5f;
    [SerializeField] private Image fadeImage;

    void Start()
    {
        
    }

    void Update()
    {
        fadeImage.color = ColorHelpers.SetColorAlpha(fadeImage.color, fadeImage.color.a + fadeSpeed * Time.deltaTime);
    }
}
