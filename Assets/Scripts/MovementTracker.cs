using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTracker : MonoBehaviour
{
    [SerializeField] private Transform movementSource;
    [SerializeField, Range(0.001f, 1)] private float minMoveThreshold = 0.01f;
    [SerializeField, Range(0.1f, 10)] private float trackTime = 2;
    [SerializeField] private GameObject visualizerPrefab;
    [SerializeField] private Transform trackerParent;
    private List<GameObject> pointsList = new List<GameObject>();
    private float distance;

    void Update()
    {
        if (pointsList.Count == 0)
        {
            AddPosition(movementSource.position);
            // StartCoroutine("RemovePosition");
            return;
        }
        distance = Vector3.Distance(pointsList[pointsList.Count - 1].transform.position, movementSource.position);
        if (distance > minMoveThreshold) // runtime check // ALSO DISTANCE CXHECK for too large
        {
            if (distance >= minMoveThreshold * 2)
            {
                Vector3 lastPos = pointsList[pointsList.Count - 1].transform.position;
                int overshots = ((int)(distance / minMoveThreshold)) + 1;
                for (int i = 1; i < overshots; i++)
                {
                    AddPosition(Vector3.Lerp(lastPos, movementSource.position, (float)(i) / (float)(overshots)));
                }
            }
            else AddPosition(movementSource.position);
        }
    }

    private void AddPosition(Vector3 pos)
    {
        GameObject newPoint = Instantiate(visualizerPrefab, pos, visualizerPrefab.transform.rotation, trackerParent);
        if (pointsList.Count > 0)
        {
            newPoint.GetComponent<LineRenderer>().SetPositions(new Vector3[] {
                (pointsList[pointsList.Count - 1].transform.position - pos) / ((newPoint.transform.lossyScale.x + newPoint.transform.lossyScale.y + newPoint.transform.lossyScale.z) / 3),
                Vector3.zero });
        }
        pointsList.Add(newPoint);
        StartCoroutine("RemovePosition");
    }

    private IEnumerator RemovePosition()
    {
        yield return new WaitForSeconds(trackTime);
        Destroy(pointsList[0]);
        pointsList.Remove(pointsList[0]);
    }

    public List<GameObject> GetPoints()
    {
        return pointsList;
    }
}
