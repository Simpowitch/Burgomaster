using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    [HideInInspector]
    public Path path;

    public PathVisualizer pathVisualizer;

    [Header("Creation Settings")]
    public float pathWidth = 2f;
    [Range(0.5f, 1.5f)]
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

    public void UpdateRoad()
    {
        Vector2[] points = path.CalculateEvenlySpacedPoints(spacing);
        pathVisualizer.Setup(points, path.IsClosed, pathWidth);
    }
}
