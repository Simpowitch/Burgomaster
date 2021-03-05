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

    public void UpdatePath()
    {
        Vector2[] points = path.CalculateEvenlySpacedPoints(spacing);
        pathVisualizer.Setup(points, path.IsClosed, pathWidth);
    }

    public void SetSelected(bool selected)
    {
        pathVisualizer.SetSelected(selected);
    }
}
