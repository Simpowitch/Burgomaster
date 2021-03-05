using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LinePathVisualizer : PathVisualizer
{
    LineRenderer lineRenderer;

    public override void Setup(Vector2[] points, bool isClosed, float pathWidth)
    {
        base.Setup(points, isClosed, pathWidth);

        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startWidth = pathWidth;
        lineRenderer.endWidth = pathWidth;

        Vector3[] p3d = Utility.Vector2ToVector3Array(points);
        lineRenderer.positionCount = p3d.Length;
        lineRenderer.SetPositions(p3d);
    }
}
