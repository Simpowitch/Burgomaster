using UnityEngine;

public abstract class PathVisualizer : MonoBehaviour
{
    public abstract void Setup(Vector2[] points, bool isClosed, float pathWidth);
}
