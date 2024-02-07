using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTimerCause : ACause
{
    [SerializeField] private float initialDelay = 1;
    [SerializeField] private float secondsBetween = 1;
    [SerializeField] private int maxTimes = -1;
    private int timesOccurred = 0;

    private void Start()
    {
        StartCoroutine(InitialDelay());
    }

    private IEnumerator InitialDelay()
    {
        yield return new WaitForSeconds(initialDelay);
        CauseEffects();
        if (maxTimes < 0 || !Continue()) yield break;
        StartCoroutine(InitialDelay());
    }

    private IEnumerator DelayedEffect()
    {
        yield return new WaitForSeconds(secondsBetween);
        CauseEffects();
        if (maxTimes < 0 || !Continue()) yield break;
        StartCoroutine(DelayedEffect());
    }
    private bool Continue()
    {
        timesOccurred++;
        if (maxTimes > 0 && timesOccurred >= maxTimes)
        {
            return false;
        }
        return true;
    }
}
