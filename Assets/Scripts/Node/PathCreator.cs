using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PathCreator : MonoBehaviour
{
    [HideInInspector]
    public Path path;

    public float pathWidth = 2f;
    [Range(0.5f, 1.5f)]
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
    public bool clampUV;

    private void Reset()
    {
        CreatePath();
    }

    public void CreatePath()
    {
        path = new Path(transform.position);
    }

    public void UpdateRoad()
    {
        Vector2[] points = path.CalculateEvenlySpacedPoints(spacing);
        GetComponent<MeshFilter>().mesh = CreatePathMesh(points, path.IsClosed, clampUV);
    }

    Mesh CreatePathMesh(Vector2[] points, bool isClosed, bool clampUVs)
    {
        Vector3[] verts = new Vector3[points.Length * 2];
        Vector2[] uvs = new Vector2[verts.Length];

        int numTris = 2 * (points.Length - 1) + (isClosed ? 2 : 0);

        int[] tris = new int[numTris * 3];
        int vertIndex = 0;
        int triIndex = 0;

        for (int i = 0; i < points.Length; i++)
        {
            Vector2 forward = Vector2.zero;
            if (i < points.Length - 1 || isClosed)
            {
                forward += points[(i + 1) % points.Length] - points[i];
            }
            if (i > 0 || isClosed)
            {
                forward += points[i] - points[(i - 1 + points.Length) % points.Length];
            }
            forward.Normalize();

            Vector2 left = new Vector2(-forward.y, forward.x);

            verts[vertIndex] = points[i] + left * pathWidth * 0.5f;
            verts[vertIndex + 1] = points[i] - left * pathWidth * 0.5f;

            if (clampUVs)
            {
                //Uvs
                float completionPercent = i / (float)(points.Length - 1);
                float v = 1 - Mathf.Abs(2 * completionPercent - 1);
                uvs[vertIndex] = new Vector2(0, v);
                uvs[vertIndex + 1] = new Vector2(1, v);
            }

            if (i < points.Length - 1 || isClosed)
            {
                tris[triIndex] = vertIndex;
                tris[triIndex + 1] = (vertIndex + 2) % verts.Length;
                tris[triIndex + 2] = vertIndex + 1;

                tris[triIndex + 3] = vertIndex + 1;
                tris[triIndex + 4] = (vertIndex + 2) % verts.Length;
                tris[triIndex + 5] = (vertIndex + 3) % verts.Length;
            }

            vertIndex += 2;
            triIndex += 6;
        }

        if (!clampUVs)
        {
            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2(verts[i].x, verts[i].y);
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;

        return mesh;
    }
}
