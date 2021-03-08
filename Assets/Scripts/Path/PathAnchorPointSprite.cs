using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PathAnchorPointSprite : PathAnchorPoint
{
    public float scaleMultiplier = 1f;

    public override void Setup(PathCreator creator, Path path, int pointIndex, Vector2 startPos, float width)
    {
        base.Setup(creator, path, pointIndex, startPos, width);

        width *= scaleMultiplier;
        this.transform.localScale = new Vector3(width, width, 1);
    }
}
