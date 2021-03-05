using UnityEngine;
using UnityEngine.Events;

public abstract class PathVisualizer : MonoBehaviour
{
    public LineRenderer helperLine;

    public UnityEvent OnSelection, OnDeselection;

    public virtual void Setup(Vector2[] points, bool isClosed, float pathWidth)
    {
        if (helperLine)
        {
            Vector3[] p3d = Utility.Vector2ToVector3Array(points);
            helperLine.positionCount = p3d.Length;
            helperLine.SetPositions(p3d);
        }
    }

    public void SetSelected(bool selected)
    {
        if (selected)
            OnSelection?.Invoke();
        else
            OnDeselection?.Invoke();
    }
}
