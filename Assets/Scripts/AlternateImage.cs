using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlternateImage : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private List<Sprite> images = new List<Sprite>();
    private int index = 0;
    [SerializeField] private float swapTime = 1;

    void Start()
    {
        StartCoroutine("NextImage");
    }

    private IEnumerator NextImage()
    {
        index++;
        if (index >= images.Count) index = 0;
        image.sprite = images[index];
        yield return new WaitForSeconds(swapTime);
        StartCoroutine("NextImage");
    }
}
