using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class OrbSpawner : MonoBehaviour
{
    [SerializeField] private bool input = true;

    [SerializeField] private GameObject waterOrb;
    [SerializeField] private GameObject earthOrb;
    [SerializeField] private GameObject fireOrb;
    [SerializeField] private GameObject airOrb;

    [SerializeField] private Transform spawnPos;
    [SerializeField] private Transform normalParent;
    [SerializeField] private Transform movingParent;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform handT;
    private Transform currentParent;
    private bool moving = false;
    private GameObject currentOrb = null;
    private float CD = 0;

    private List<Vector3> movementChecker = new List<Vector3>();
    [SerializeField] private int maxMovementChecks = 3;

    void Start()
    {
        
    }

    void Update()
    {
        if (CD <= 0 && input) OrbInputCheck();

        /*if (!moving && VRInput.ButtonPressed(XRNode.RightHand, InputHelpers.Button.Grip))
        {
            moving = true;
            currentParent = movingParent;
            if (currentOrb != null) currentOrb.transform.parent = currentParent.transform;
        }*/
    }

    private void FixedUpdate()
    {
        if (movementChecker.Count > maxMovementChecks) movementChecker.RemoveAt(0);
        if (currentOrb != null) movementChecker.Add(currentOrb.transform.position);

        if (!input && moving && !VRInput.ButtonPressed(XRNode.RightHand, InputHelpers.Button.Grip))
        {
            moving = false;
            currentParent = normalParent;
            // if (currentOrb != null) currentOrb.transform.parent = currentParent.transform;
            if (currentOrb != null)
            {
                currentOrb.transform.parent = null;
                /*Vector3 sum = new Vector3();
                foreach (Vector3 v in movementChecker)
                {
                    sum += v;
                }
                currentOrb.GetComponent<Rigidbody>().velocity = (sum / movementChecker.Count) / (Time.fixedDeltaTime * maxMovementChecks);*/
                Vector3 startPos = movementChecker[movementChecker.Count - 1];
                Vector3 endPos = movementChecker[0];
                currentOrb.GetComponent<Orb>().ReleasedFromHand((startPos - endPos) / (Time.fixedDeltaTime * maxMovementChecks));
            }
        }
    }

    private void OrbInputCheck()
    {
        /*if (VRInput.ButtonPressed(XRNode.RightHand, InputHelpers.Button.PrimaryButton))
        {
            CreateOrb(earthOrb);
        }
        if (VRInput.ButtonPressed(XRNode.RightHand, InputHelpers.Button.SecondaryButton))
        {
            CreateOrb(airOrb);
        }
        if (VRInput.ButtonPressed(XRNode.LeftHand, InputHelpers.Button.PrimaryButton))
        {
            CreateOrb(waterOrb);
        }
        if (VRInput.ButtonPressed(XRNode.LeftHand, InputHelpers.Button.SecondaryButton))
        {
            CreateOrb(fireOrb);
        }*/
    }

    public void CreateOrb(GameObject prefab)
    {
        if (VRInput.ButtonPressed(XRNode.RightHand, InputHelpers.Button.Grip))
        {
            moving = true;
            currentParent = movingParent;
        }

        if (currentOrb != null) Destroy(currentOrb);
        if (prefab != null)
        {
            currentOrb = Instantiate(prefab, spawnPos.position, Quaternion.identity, currentParent);
            currentOrb.GetComponent<Spell>().SpellInit(handT.GetComponent<PlayerHand>());
        }
        StartCoroutine("Cooldown");
    }

    private IEnumerable Cooldown()
    {
        CD = 1;
        yield return new WaitForSeconds(1);
        CD = 0;
    }
}
