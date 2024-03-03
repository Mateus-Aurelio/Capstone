using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Recalibrate : MonoBehaviour
{
    [SerializeField] private GameObject calibrationCanvas;
    [SerializeField] private Transform leftHandT;
    [SerializeField] private Transform rightHandT;
    [SerializeField] private Transform cameraT;
    [SerializeField] private Transform feetT;
    private bool calibrating = false;
    private bool midCalibrating = false;

    private Vector3 extendedLeftHandPos; 
    private Vector3 extendedRightHandPos; 
    private Vector3 loweredLeftHandPos; 
    private Vector3 loweredRightHandPos; 
    private Vector3 leftShoulderPos; 
    private Vector3 rightShoulderPos; 
    private Vector3 corePos; 
    private float armsLength; 
    private float height;

    [SerializeField] private GameObject visualizationPrefab;

    private void Start()
    {
        EnterCalibrationMode();
    }

    private void Update()
    {
        if (!calibrating && VRInput.ButtonPressed(XRNode.LeftHand, InputHelpers.Button.MenuButton))
        {
            EnterCalibrationMode();
            return;
        }
        if (!midCalibrating && calibrating && VRInput.ButtonPressed(XRNode.LeftHand, InputHelpers.Button.Grip) && VRInput.ButtonPressed(XRNode.RightHand, InputHelpers.Button.Grip))
        {
            midCalibrating = true;
            extendedLeftHandPos = leftHandT.position - cameraT.position;
            extendedRightHandPos = rightHandT.position - cameraT.position;
            return;
        }
        if (midCalibrating)
        {
            Calibration();
            if (!VRInput.ButtonPressed(XRNode.LeftHand, InputHelpers.Button.Grip) && !VRInput.ButtonPressed(XRNode.RightHand, InputHelpers.Button.Grip))
            {
                ExitCalibrationMode();
            }
        }
    }

    private void EnterCalibrationMode()
    {
        calibrationCanvas.SetActive(true);
        calibrating = true;
    }

    private void Calibration()
    {

    }

    private void ExitCalibrationMode()
    {
        calibrationCanvas.SetActive(false);
        calibrating = false; 
        midCalibrating = false; 

        loweredLeftHandPos = leftHandT.position - cameraT.position; 
        loweredRightHandPos = rightHandT.position - cameraT.position;

        leftShoulderPos = new Vector3(loweredLeftHandPos.x, extendedLeftHandPos.y, loweredLeftHandPos.z);
        rightShoulderPos = new Vector3(loweredRightHandPos.x, extendedRightHandPos.y, loweredRightHandPos.z);
        BodyData.leftShoulder = leftShoulderPos;
        BodyData.rightShoulder = rightShoulderPos;
        BodyData.shouldersCenter = new Vector3(0, (rightShoulderPos.y + leftShoulderPos.y) / 2, 0);

        corePos = new Vector3(0, (leftShoulderPos.y + rightShoulderPos.y + loweredLeftHandPos.y + loweredRightHandPos.y) / 4, 0);
        BodyData.core = corePos;

        armsLength = Vector3.Distance(leftShoulderPos, extendedLeftHandPos);
        armsLength += Vector3.Distance(leftShoulderPos, loweredLeftHandPos);
        armsLength += Vector3.Distance(rightShoulderPos, extendedRightHandPos);
        armsLength += Vector3.Distance(rightShoulderPos, loweredRightHandPos);
        armsLength /= 4;
        BodyData.armsLength = armsLength;

        if (visualizationPrefab != null)
        {
            Instantiate(visualizationPrefab, leftShoulderPos, visualizationPrefab.transform.rotation);
            Instantiate(visualizationPrefab, rightShoulderPos, visualizationPrefab.transform.rotation);
            Instantiate(visualizationPrefab, extendedLeftHandPos, visualizationPrefab.transform.rotation);
            Instantiate(visualizationPrefab, extendedRightHandPos, visualizationPrefab.transform.rotation);
            Instantiate(visualizationPrefab, loweredLeftHandPos, visualizationPrefab.transform.rotation);
            Instantiate(visualizationPrefab, loweredRightHandPos, visualizationPrefab.transform.rotation);
            Instantiate(visualizationPrefab, corePos, visualizationPrefab.transform.rotation);
        }
        Debug.Log("extendedLeftHandPos " + extendedLeftHandPos);
        Debug.Log("extendedRightHandPos " + extendedRightHandPos);
        Debug.Log("loweredLeftHandPos " + loweredLeftHandPos);
        Debug.Log("loweredRightHandPos " + loweredRightHandPos);
        Debug.Log("leftShoulderPos " + leftShoulderPos);
        Debug.Log("rightShoulderPos " + rightShoulderPos);
        Debug.Log("corePos " + corePos);
        Debug.Log("armsLength " + armsLength);

        /*Debug.Log("Feet pos: " + feetT.position);
        Debug.Log("Camera pos: " + cameraT.position);
        Debug.Log("Right hand pos: " + rightHandT.position);
        Debug.Log("Left hand pos: " + leftHandT.position);
        float lefthandDistance = Vector3.Distance(leftHandT.position, cameraT.position);
        float righthandDistance = Vector3.Distance(rightHandT.position, cameraT.position);
        Debug.Log("Right hand distance: " + righthandDistance);
        Debug.Log("Left hand distance: " + lefthandDistance);
        Vector3 handAverage = (rightHandT.position + leftHandT.position) / 2;
        Debug.Log("Average Hand Position: " + handAverage);

        height = cameraT.position.y - feetT.position.y;
        Debug.Log("Height: " + height);
        chestShoulderPos = new Vector3(feetT.position.x, handAverage.y, feetT.position.z);
        Debug.Log("chestShoulderPos: " + chestShoulderPos);
        lefthandDistance = Vector3.Distance(leftHandT.position, chestShoulderPos);
        righthandDistance = Vector3.Distance(rightHandT.position, chestShoulderPos);
        armsLength = (lefthandDistance + righthandDistance) / 2;
        Debug.Log("armsLength: " + armsLength);*/
    }
}

public static class BodyData
{
    public static Vector3 leftShoulder;
    public static Vector3 rightShoulder;
    public static Vector3 core;
    public static Vector3 shouldersCenter;
    public static float armsLength;
}
