using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using PDollarGestureRecognizer;
using TMPro;

public class MovementRecognizer : MonoBehaviour
{
    [SerializeField] private XRNode inputSource;
    [SerializeField] private InputHelpers.Button inputButton;
    [SerializeField] private float inputThreshold = 0.1f;
    [SerializeField] private Transform movementSource;
    [SerializeField] private float minMoveThreshold = 0.1f;
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private Transform pointsParent;
    private bool isMoving = false;
    private bool isPressed = false;
    private List<GameObject> pointsList = new List<GameObject>();
    private List<Gesture> gestures = new List<Gesture>();
    [SerializeField] private TextMeshProUGUI outputlol;
    [SerializeField] private List<TextAsset> gestureFiles = new List<TextAsset>();
    [SerializeField] private float scoreMin = 0.9f;
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private GameObject trianglePrefab;
    [SerializeField] private GameObject triangle2Prefab;

    private void Start()
    {
        /*string[] filePaths = Directory.GetFiles(Application.dataPath + "/Gestures/", "*.xml");
        foreach (string filePath in filePaths)
            gestures.Add(GestureIO.ReadGestureFromFile(filePath));*/

        /*string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml"); // runtime check 
        foreach (var file in gestureFiles)
        {
            gestures.Add(GestureIO.ReadGestureFromFile(file));
        }*/

        // TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("Gestures/");
		foreach (TextAsset gestureXml in gestureFiles)
            gestures.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

        outputlol.text = "Gestures found: " + gestures.Count + " from " + (Application.dataPath + "/Gestures/");
    }

    private void Update()
    {
        InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out isPressed, inputThreshold);

        if (!isMoving && isPressed)
        {
            StartMovement();
        }
        else if (isMoving && !isPressed)
        {
            EndMovement();
        }
        else if (isMoving && isPressed)
        {
            UpdateMovement();
        }
    }

    private void StartMovement()
    {
        outputlol.text = "Movement started.";
        SetMovement(true);

        foreach (GameObject point in pointsList)
        {
            Destroy(point);
        }
        pointsList.Clear();
        AddPosition(movementSource.position);
    }

    private void EndMovement()
    {
        SetMovement(false);
        outputlol.text = "Movement ended. ";

        List<Point> points = new List<Point>();
        List<Vector3> positions = new List<Vector3>();
        Vector2 screenPoint;
        foreach (GameObject g in pointsList)
        {
            // positions.Add(g.transform.position);
            screenPoint = Camera.main.WorldToScreenPoint(g.transform.position);
            points.Add(new Point(screenPoint.x, screenPoint.y, 0));
        }
        // points = ShapeHelper.PlanifiedPoints(positions);

        foreach (GameObject point in pointsList)
        {
            Destroy(point);
        }
        pointsList.Clear();

        Result result = PointCloudRecognizer.Classify(new Gesture(points.ToArray()), gestures.ToArray());
        if (result.Score < scoreMin)
        {
            outputlol.text = "Bad score: " + result.Score + ", class: " + result.GestureClass + ". Points.Count = " + points.Count + ", pointsList.Count = " + pointsList.Count;
            Debug.Log("Bad score: " + result.Score + ", class: " + result.GestureClass);
            return;
        }

        outputlol.text = result.GestureClass + ", " + result.Score;

        switch (result.GestureClass)
        {
            case "Circle":
                Instantiate(circlePrefab, movementSource.transform.position, Quaternion.identity);
                break;

            case "Square":
                Instantiate(squarePrefab, movementSource.transform.position, Quaternion.identity);
                break;

            case "Triangle":
                Instantiate(trianglePrefab, movementSource.transform.position, Quaternion.identity);
                break;

            case "Triangle2":
                Instantiate(triangle2Prefab, movementSource.transform.position, Quaternion.identity);
                break;

            default:
                outputlol.text = "Unknown gesture class: " + result.GestureClass + ", score of " + result.Score;
                Debug.Log("Unknown gesture class: " + result.GestureClass + ", score of " + result.Score);
                break;
        }
    }

    private void SetMovement(bool given)
    {
        isMoving = given;
    }

    private void UpdateMovement()
    {
        if (Vector3.Distance(pointsList[pointsList.Count - 1].transform.position, movementSource.position) > minMoveThreshold) // runtime check // ALSO DISTANCE CXHECK for too large
        {
            AddPosition(movementSource.position);
        }
    }

    private void AddPosition(Vector3 pos)
    {
        GameObject newPoint = Instantiate(pointPrefab, pos, pointPrefab.transform.rotation, pointsParent);
        if (pointsList.Count > 0)
        {
            newPoint.GetComponent<LineRenderer>().SetPositions(new Vector3[] {
                (pointsList[pointsList.Count - 1].transform.position - pos) / ((newPoint.transform.lossyScale.x + newPoint.transform.lossyScale.y + newPoint.transform.lossyScale.z) / 3),
                Vector3.zero });
        }
        pointsList.Add(newPoint);
    }
}

public static class ShapeHelper
{

    public static List<Point> PlanifiedPoints(List<Vector3> positions)
    {
        Vector3 centerPoint = new Vector3(0, 0, 0);
        Vector3 maxValues = positions[0];
        Vector3 minValues = positions[0];
        Vector3 avgValues = new Vector3(0, 0, 0);

        avgValues = Vector3.zero;
        foreach (Vector3 v in positions)
        {
            if (v.x > maxValues.x) maxValues.x = v.x;
            else if (v.x < minValues.x) minValues.x = v.x;
            if (v.y > maxValues.y) maxValues.y = v.y;
            else if (v.y < minValues.y) minValues.y = v.y;
            if (v.z > maxValues.z) maxValues.z = v.z;
            else if (v.z < minValues.z) minValues.z = v.z;
            avgValues += v;
        }
        avgValues /= positions.Count;
        centerPoint = (maxValues + minValues) / 2;

        Matrix4x4 m4 = CalculateCovarianceMatrix(positions, avgValues);
        Vector3 normal = FindPlaneNormal(m4);
        List<Vector3> planified = PlaneProjection.ProjectPointsOntoPlane(positions, avgValues, normal);

        /*Vector3 referencePos = avgValues + normal * 10;
        List<Point> newPoints = new List<Point>();
        foreach (Vector3 pos in positions)
        {
            screenPoint = Camera.main.WorldToScreenPoint(g.transform.position);
            newPoints.Add(new Point(screenPoint.x, screenPoint.y, 0));
        }*/
        return PlaneProjection.Convert3DPointsTo2D(planified, normal);
    }

    public static Matrix4x4 CalculateCovarianceMatrix(List<Vector3> points, Vector3 centroid)
    {
        Matrix4x4 covarianceMatrix = new Matrix4x4();
        foreach (Vector3 point in points)
        {
            Vector3 diff = point - centroid;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    covarianceMatrix[i, j] += diff[i] * diff[j];
                }
            }
        }
        return covarianceMatrix;
    }

    public static Vector3 FindPlaneNormal(Matrix4x4 covarianceMatrix)
    {
        // This is a simplified computation that assumes the smallest eigenvalue
        // is the last one. For more accuracy, you might need to use a numerical
        // library to compute eigenvalues and eigenvectors.
        Vector3 normal = new Vector3(covarianceMatrix[0, 2], covarianceMatrix[1, 2], covarianceMatrix[2, 2]).normalized;
        return normal;
    }


}



public class PlaneProjection : MonoBehaviour
{
    public static List<Vector3> ProjectPointsOntoPlane(List<Vector3> points, Vector3 planePoint, Vector3 planeNormal)
    {
        List<Vector3> projectedPoints = new List<Vector3>();
        foreach (Vector3 point in points)
        {
            projectedPoints.Add(ProjectPointOntoPlane(point, planePoint, planeNormal));
        }
        return projectedPoints;
    }

    private static Vector3 ProjectPointOntoPlane(Vector3 point, Vector3 planePoint, Vector3 planeNormal)
    {
        // Calculate the distance from the point to the plane
        float distance = Vector3.Dot(planeNormal, (point - planePoint));

        // Project the point onto the plane
        Vector3 projectedPoint = point - distance * planeNormal;
        return projectedPoint;
    }

    public static List<Point> Convert3DPointsTo2D(List<Vector3> pointsOnPlane, Vector3 planeNormal)
    {
        // Create two orthogonal vectors on the plane
        Vector3 basisU = Vector3.right - planeNormal * Vector3.Dot(Vector3.right, planeNormal);
        Vector3 basisV = Vector3.Cross(planeNormal, basisU);

        // Normalize the basis vectors
        basisU.Normalize();
        basisV.Normalize();

        List<Point> points2D = new List<Point>();
        foreach (Vector3 point in pointsOnPlane)
        {
            // Project the 3D point onto the 2D basis vectors
            float x = Vector3.Dot(point, basisU);
            float y = Vector3.Dot(point, basisV);
            points2D.Add(new Point(x, y, 0));
        }
        return points2D;
    }
}