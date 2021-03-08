using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathAnchorPoint : MonoBehaviour
{
    PathCreator creator;
    Path path;
    int pointIndex;
    public SpriteRenderer handle;

    public virtual void Setup(PathCreator creator, Path path, int pointIndex, Vector2 startPos, float width)
    {
        this.creator = creator;
        this.path = path;
        this.pointIndex = pointIndex;
        MovePoint(startPos);
    }

    public void ShowHandle(bool show) => handle.enabled = show;

    public void MovePoint(Vector2 newPos)
    {
        this.transform.position = newPos;
        path.MovePoint(pointIndex, newPos);
        creator.UpdatePath();
    }

    public void Delete()
    {
        path.DeleteSegment(pointIndex);
        creator.UpdatePath();
        Destroy(this.gameObject);
    }
}
