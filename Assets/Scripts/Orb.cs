using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    private GameObject myHand;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetHandObject(GameObject givenHand)
    {
        myHand = givenHand;
    }

    public void ReleasedFromHand(Vector3 velocity)
    {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHand") && other.gameObject != myHand)
        {
            // other.gameObject.GetComponent<PlayerHand>().EnterTouchSpellMode();

            Destroy(gameObject, 0.01f);
        }
    }
}
