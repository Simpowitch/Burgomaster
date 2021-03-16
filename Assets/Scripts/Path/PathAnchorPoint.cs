using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathAnchorPoint : MonoBehaviour
{
    private static PathAnchorPoint selection;
    public static PathAnchorPoint Selection
    {
        get => selection;
        set
        {
            if (selection)
                selection.DeSelect();
            selection = value;
            if (selection)
                selection.Select();
        }
    }

    PathCreator creator;
    Path path;
    int pointIndex;
    public SpriteRenderer handle = null;
    public SpriteRenderer handleOutline = null;

    public virtual void Setup(PathCreator creator, Path path, int pointIndex, Vector2 startPos, float width)
    {
        this.creator = creator;
        this.path = path;
        this.pointIndex = pointIndex;
        MovePoint(startPos);

        handleOutline.material = handleOutline.material;
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

    private void Select()
    {
        handleOutline.material.SetInt(Shader.PropertyToID("_ShowOutline"), 1);
        handleOutline.material.SetInt(Shader.PropertyToID("_IsPulsing"), 0);
    }
    private void DeSelect()
    {
        handleOutline.material.SetInt(Shader.PropertyToID("_ShowOutline"), 0);
    }

    private void HooverStart()
    {
        handleOutline.material.SetInt(Shader.PropertyToID("_IsPulsing"), 1);
        handleOutline.material.SetInt(Shader.PropertyToID("_ShowOutline"), 1);
    }

    private void HooverEnd()
    {
        handleOutline.material.SetInt(Shader.PropertyToID("_IsPulsing"), 0);
        handleOutline.material.SetInt(Shader.PropertyToID("_ShowOutline"), 0);
    }

    void OnMouseEnter()
    {
        if (this != Selection)
        {
            HooverStart();
        }
    }

    void OnMouseExit()
    {
        if (this != Selection)
        {
            HooverEnd();
        }
    }
}
