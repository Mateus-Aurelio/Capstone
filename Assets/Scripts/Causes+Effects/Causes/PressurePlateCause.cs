using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateCause : ACause
{
    [SerializeField] private Transform topPlateT; 
    [SerializeField] private float restingYPos = 0.5f; 
    [SerializeField] private float activatedYPos = 0.0f; 
    [SerializeField] private float timeToActivate = 0.1f; 
    [SerializeField] private bool causeEveryFrame = false; 
    [SerializeField] private bool continuedCheck = false; 
    private float timeActive = 0;
    private bool activated = false;

    void Update()
    {
        if (topPlateT.position.y <= activatedYPos)
        {
            if (!causeEveryFrame && activated) return;
            timeActive += Time.deltaTime;
            if (timeActive > timeToActivate)
            {
                CauseEffects();
                activated = true;
            }
        }
        else
        {
            timeActive = 0;
            activated = false;
        }
    }
}
