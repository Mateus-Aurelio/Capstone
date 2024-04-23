using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerHead : MonoBehaviour
{
    [SerializeField] private Transform playerHead;
    [SerializeField] private bool followYPos = false;

    private void LateUpdate()
    {
        if (followYPos) transform.position = playerHead.position;
        else transform.position = new Vector3(playerHead.position.x, transform.position.y, playerHead.position.z);
    }
}
