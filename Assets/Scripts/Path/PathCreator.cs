using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    [HideInInspector]
    public Path path;

    public PathVisualizer pathVisualizer;
    public PathAnchorPoint anchorPointBP;

    List<PathAnchorPoint> pathAnchorPoints = new List<PathAnchorPoint>();
    public PathAnchorPoint LastAnchorPoint => pathAnchorPoints[pathAnchorPoints.Count - 1];
    public bool CanRemoveAnchorPoint => pathAnchorPoints.Count > 2;

    [Header("Creation Settings")]
    public float pathWidth = 2f;
    [Range(0.1f, 1f)]
    public float spacing = 1f;

    [Header("Gizmos and Editor")]
    public Color anchorColor = Color.red;
    public Color controlColor = Color.white;
    public Color segmentColor = Color.green;
    public Color selectedSegmentColor = Color.yellow;

    public float anchorDiameter = 0.1f;
    public float controlDiameter = 0.075f;
    public bool displayControlPoints = true;
    public bool autoUpdate;

    private void Reset()
    {
        CreatePath();
    }

    public void CreatePath()
    {
        path = new Path(transform.position);
    }

    public void CreatePath(Vector2 position)
    {
        path = new Path(position);
    }

    public PathCreator Setup(Vector2 pos)
    {
        //Adds a segment automatically with 2 anchor points
        CreatePath(pos);

        //Create point 0
        PathAnchorPoint newAnchor = Instantiate(anchorPointBP, this.transform);
        newAnchor.Setup(this, path, 0, pos, pathWidth);

        pathAnchorPoints.Add(newAnchor);

        //Create point 1
        newAnchor = Instantiate(anchorPointBP, this.transform);
        newAnchor.Setup(this, path, path.LastAnchor, pos, pathWidth);

        pathAnchorPoints.Add(newAnchor);

        return this;
    }

    public void SetSelected(bool selected)
    {
        pathVisualizer.SetSelected(selected);
        foreach (var anchorPoint in pathAnchorPoints)
        {
            anchorPoint.ShowHandle(selected);
        }
    }

    public void UpdatePath()
    {
        Vector2[] points = path.CalculateEvenlySpacedPoints(spacing);
        pathVisualizer.Setup(points, path.IsClosed, pathWidth);
    }

    

    public void AddSegment(Vector2 pos)
    {
        path.AddSegment(pos);

        PathAnchorPoint newAnchor = Instantiate(anchorPointBP, this.transform);
        newAnchor.Setup(this, path, path.LastAnchor, pos, pathWidth);

        pathAnchorPoints.Add(newAnchor);
    }

    public void DeleteLastSegment()
    {
        LastAnchorPoint.Delete();
        pathAnchorPoints.Remove(LastAnchorPoint);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
