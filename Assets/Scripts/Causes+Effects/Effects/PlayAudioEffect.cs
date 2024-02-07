using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioEffect : AEffect
{
    [SerializeField] AudioSource audioSource;

    public override void DoEffect()
    {
        audioSource.Play();
    }
}
