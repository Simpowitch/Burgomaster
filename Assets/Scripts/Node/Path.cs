using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path
{
    [SerializeField, HideInInspector]
    List<Vector2> points;
    [SerializeField, HideInInspector]
    bool isClosed;
    [SerializeField, HideInInspector]
    bool autoSetControlPoints = true;

    public Path(Vector2 centre)
    {
        points = new List<Vector2>
        {
            centre+Vector2.left,
            centre+(Vector2.left + Vector2.up) * 0.5f,
            centre+(Vector2.right + Vector2.down) * 0.5f,
            centre+Vector2.right
        };
    }

    public Vector2 this[int i] => points[i];

    public bool AutoSetControlPoints
    {
        get => autoSetControlPoints;
        set
        {
            if (autoSetControlPoints != value)
            {
                autoSetControlPoints = value;
                if (autoSetControlPoints)
                {
                    AutoSetAllControlPoints();
                }
            }
        }
    }

    public bool IsClosed
    {
        get => isClosed;
        set
        {
            if (isClosed != value)
            {
                isClosed = value;

                if (isClosed)
                {
                    points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
                    points.Add(points[0] * 2 - points[1]);

                    if (autoSetControlPoints)
                    {
                        AutoSetAnchorControlPoints(0);
                        AutoSetAnchorControlPoints(points.Count - 3);
                    }
                }
                else
                {
                    points.RemoveRange(points.Count - 2, 2);

                    if (autoSetControlPoints)
                    {
                        AutoSetStartAndEndControls();
                    }
                }
            }
        }
    }

    public int NumberOfSegments => points.Count / 3;
    public int NumberOfPoints => points.Count;

    public void AddSegment(Vector2 anchorPos)
    {
        points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
        points.Add((points[points.Count - 1] + anchorPos) * 0.5f);
        points.Add(anchorPos);

        if (autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints(points.Count - 1);
        }
    }

    public void SplitSegment(Vector2 anchorPos, int segmentIndex)
    {
        points.InsertRange(segmentIndex * 3 + 2, new Vector2[] { Vector2.zero, anchorPos, Vector2.zero });
        if (autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints(segmentIndex * 3 + 3);
        }
        else
        {
            AutoSetAnchorControlPoints(segmentIndex * 3 + 3);
        }
    }

    public void DeleteSegment(int anchorIndex)
    {
        //Minimum requirements
        if (NumberOfSegments <= 2 || isClosed && NumberOfSegments <= 1)
            return;

        if (anchorIndex == 0) //First anchor
        {
            if (isClosed)
            {
                points[points.Count - 1] = points[2];
            }
            points.RemoveRange(0, 3);
        }
        else if (anchorIndex == points.Count - 1 && !isClosed) //Last anchor
        {
            points.RemoveRange(anchorIndex - 2, 3);
        }
        else //Default
        {
            points.RemoveRange(anchorIndex - 1, 3);
        }
    }

    public Vector2[] GetPointsInSegment(int index)
    {
        return new Vector2[] { points[index * 3], points[index * 3 + 1], points[index * 3 + 2], points[LoopIndex(index * 3 + 3)] };
    }

    public void MovePoint(int index, Vector2 newPos)
    {
        //Forbid movement of controlpoints if autoSetControlPoints is turned on
        if (index % 3 != 0 && autoSetControlPoints)
            return;

        Vector2 deltaMove = newPos - points[index];
        points[index] = newPos;

        if (autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints(index);
        }
        else
        {
            //Moving an anchor point
            if (index % 3 == 0)
            {
                if (index + 1 < points.Count || isClosed)
                    points[LoopIndex(index + 1)] += deltaMove;
                if (index - 1 >= 0 || isClosed)
                    points[LoopIndex(index - 1)] += deltaMove;
            }
            else
            {
                bool nextPointIsAnchor = (index + 1) % 3 == 0;
                int correspondingControlIndex = (nextPointIsAnchor) ? index + 2 : index - 2;
                int anchorIndex = (nextPointIsAnchor) ? index + 1 : index - 1;

                if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count || isClosed)
                {
                    float distance = (points[LoopIndex(anchorIndex)] - points[LoopIndex(correspondingControlIndex)]).magnitude;
                    Vector2 dir = (points[LoopIndex(anchorIndex)] - newPos).normalized;
                    points[LoopIndex(correspondingControlIndex)] = points[LoopIndex(anchorIndex)] + dir * distance;
                }
            }
        }
    }

    public Vector2[] CalculateEvenlySpacedPoints(float spacing, float resolution = 1f)
    {
        List<Vector2> evenlySpacedPoints = new List<Vector2>();
        evenlySpacedPoints.Add(points[0]);
        Vector2 previousPoint = points[0];
        float dstSinceLastEvenPoint = 0;

        for (int segment = 0; segment < NumberOfSegments; segment++)
        {
            Vector2[] p = GetPointsInSegment(segment);

            float controlNetLength = Vector2.Distance(p[0], p[1]) + Vector2.Distance(p[1], p[2]) + Vector3.Distance(p[2], p[3]);
            float estimatedCurvelength = Vector2.Distance(p[0], p[3]) + controlNetLength / 2;

            int divisions = Mathf.CeilToInt(estimatedCurvelength * resolution * 10);

            float t = 0;
            while(t<= 1)
            {
                t += 1f / divisions;
                Vector2 pointOnCurve = Bezier.EvaluateCubic(p[0], p[1], p[2], p[3], t);
                dstSinceLastEvenPoint += Vector2.Distance(previousPoint, pointOnCurve);

                while (dstSinceLastEvenPoint >= spacing)
                {
                    float overshootDst = dstSinceLastEvenPoint - spacing;
                    Vector2 newEvenlySpacedPoint = pointOnCurve + (previousPoint - pointOnCurve).normalized * overshootDst;
                    evenlySpacedPoints.Add(newEvenlySpacedPoint);
                    dstSinceLastEvenPoint = overshootDst;
                    previousPoint = newEvenlySpacedPoint;
                }

                previousPoint = pointOnCurve;
            }
        }

        return evenlySpacedPoints.ToArray();
    }

    void AutoSetAllAffectedControlPoints(int updatedAnchorIndex)
    {
        for (int i = updatedAnchorIndex - 3; i <= updatedAnchorIndex + 3; i += 3)
        {
            if (i >= 0 && i < points.Count || isClosed)
            {
                AutoSetAnchorControlPoints(LoopIndex(i));
            }
        }
        AutoSetStartAndEndControls();
    }

    void AutoSetAllControlPoints()
    {
        for (int i = 0; i < points.Count; i += 3)
        {
            AutoSetAnchorControlPoints(i);
        }
        AutoSetStartAndEndControls();
    }

    void AutoSetAnchorControlPoints(int anchorIndex)
    {
        Vector2 anchorPos = points[anchorIndex];
        Vector2 dir = Vector2.zero;
        float[] neighborDistances = new float[2];
        if (anchorIndex - 3 >= 0 || isClosed)
        {
            Vector2 offset = points[LoopIndex(anchorIndex - 3)] - anchorPos;
            dir += offset.normalized;
            neighborDistances[0] = offset.magnitude;
        }
        if (anchorIndex + 3 < points.Count || isClosed)
        {
            Vector2 offset = points[LoopIndex(anchorIndex + 3)] - anchorPos;
            dir -= offset.normalized;
            neighborDistances[1] = -offset.magnitude;
        }

        dir.Normalize();

        for (int i = 0; i < 2; i++)
        {
            int controlIndex = anchorIndex + i * 2 - 1;
            if (controlIndex >= 0 && controlIndex < points.Count || isClosed)
                points[LoopIndex(controlIndex)] = anchorPos + dir * neighborDistances[i] * 0.5f;

        }
    }

    void AutoSetStartAndEndControls()
    {
        if (!isClosed)
        {
            points[1] = (points[0] + points[2]) * 0.5f;
            points[points.Count - 2] = (points[points.Count - 1] + points[points.Count - 3]) * 0.5f;
        }
    }

    int LoopIndex(int index)
    {
        return (index + points.Count) % points.Count;
    }
}
