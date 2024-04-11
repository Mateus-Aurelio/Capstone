using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementResource : MonoBehaviour
{
    [SerializeField] private List<Image> images = new List<Image>();
    
    void Start()
    {
        
    }

    public void UpdateResource(float amount)
    {
        for (int i = 0; i < images.Count; i++)
        {
            if (amount >= i + 1) images[i].fillAmount = 1;
            else images[i].fillAmount = amount - i;
            if (images[i].fillAmount >= 1f) images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, 1);
            else images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, 0.5f);
        }
    }
}
