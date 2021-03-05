using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PathAnchorPoint : MonoBehaviour
{
    PathCreator creator;
    Path path;
    int pointIndex;
    public SpriteRenderer handle;

    public void Setup(PathCreator creator, Path path, int pointIndex, Vector2 startPos, float width)
    {
        this.creator = creator;
        this.path = path;
        this.pointIndex = pointIndex;
        MovePoint(startPos);

        this.transform.localScale = new Vector3(width, width, 1);
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
