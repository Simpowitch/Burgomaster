using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] PathCreator pathCreator = null;
    public PathAnchorPoint roadAnchorPointBP;

    List<PathAnchorPoint> pathAnchorPoints = new List<PathAnchorPoint>();
    public PathAnchorPoint LastAnchorPoint => pathAnchorPoints[pathAnchorPoints.Count - 1];
    public bool CanRemoveAnchorPoint => pathAnchorPoints.Count > 2;

    public Road Setup(Vector2 pos)
    {
        //Adds a segment automatically with 2 anchor points
        pathCreator.CreatePath(pos);

        //Create point 0
        PathAnchorPoint newAnchor = Instantiate(roadAnchorPointBP, this.transform);
        newAnchor.Setup(pathCreator, pathCreator.path, 0, pos, pathCreator.pathWidth);

        pathAnchorPoints.Add(newAnchor);

        //Create point 1
        newAnchor = Instantiate(roadAnchorPointBP, this.transform);
        newAnchor.Setup(pathCreator, pathCreator.path, pathCreator.path.LastAnchor, pos, pathCreator.pathWidth);

        pathAnchorPoints.Add(newAnchor);

        return this;
    }

    public void SetSelected(bool value)
    {
        pathCreator.SetSelected(value);

        foreach (var anchorPoint in pathAnchorPoints)
        {
            anchorPoint.ShowHandle(value);
        }
    }

    public void AddSegment(Vector2 pos)
    {
        pathCreator.path.AddSegment(pos);

        PathAnchorPoint newAnchor = Instantiate(roadAnchorPointBP, this.transform);
        newAnchor.Setup(pathCreator, pathCreator.path, pathCreator.path.LastAnchor, pos, pathCreator.pathWidth);

        pathAnchorPoints.Add(newAnchor);
    }

    public void DeleteLastSegment()
    {
        LastAnchorPoint.Delete();
        pathAnchorPoints.Remove(LastAnchorPoint);
    }

    public void DestroyRoad()
    {
        Destroy(pathCreator.gameObject);
    }
}
