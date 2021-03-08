using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class PathAnchorPointMesh : PathAnchorPoint
{
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    PolygonCollider2D polyCollider;
    public string sortingLayer;

    [Range(3, 1000)]
    public int circleVertices = 50;

    public override void Setup(PathCreator creator, Path path, int pointIndex, Vector2 startPos, float width)
    {
        base.Setup(creator, path, pointIndex, startPos, width);

        handle.gameObject.transform.localScale = new Vector3(width, width, 1);

        if (meshFilter == null)
            meshFilter = GetComponent<MeshFilter>();
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.sortingLayerName = sortingLayer;
        }
        if (polyCollider == null)
            polyCollider = GetComponent<PolygonCollider2D>();

        meshFilter.mesh = CreatePolyMesh(width / 2, circleVertices);

        if (polyCollider)
            SetupCollider();
    }

    Mesh CreatePolyMesh(float radius, int n)
    {
        Mesh mesh = new Mesh();

        //verticies
        List<Vector3> verticiesList = new List<Vector3> { };
        float x;
        float y;
        for (int i = 0; i < n; i++)
        {
            x = radius * Mathf.Sin((2 * Mathf.PI * i) / n);
            y = radius * Mathf.Cos((2 * Mathf.PI * i) / n);
            verticiesList.Add(new Vector3(x, y, 0f));
        }
        Vector3[] verts = verticiesList.ToArray();

        Vector2[] uvs = new Vector2[verts.Length];


        //triangles
        List<int> trianglesList = new List<int> { };
        for (int i = 0; i < (n - 2); i++)
        {
            trianglesList.Add(0);
            trianglesList.Add(i + 1);
            trianglesList.Add(i + 2);
        }
        int[] triangles = trianglesList.ToArray();

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(verts[i].x, verts[i].y);
        }

        //initialise
        mesh.vertices = verts;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        return mesh;
    }

    void SetupCollider()
    {
        polyCollider.pathCount = 1;

        List<Vector3> vertices = new List<Vector3>();
        meshFilter.sharedMesh.GetVertices(vertices);

        var boundaryPath = EdgeHelpers.GetEdges(meshFilter.sharedMesh.triangles).FindBoundary().SortEdges();

        Vector3[] yourVectors = new Vector3[boundaryPath.Count];
        for (int i = 0; i < boundaryPath.Count; i++)
        {
            yourVectors[i] = vertices[boundaryPath[i].v1];
        }
        List<Vector2> newColliderVertices = new List<Vector2>();

        for (int i = 0; i < yourVectors.Length; i++)
        {
            newColliderVertices.Add(new Vector2(yourVectors[i].x, yourVectors[i].y));
        }

        Vector2[] newPoints = newColliderVertices.Distinct().ToArray();

        polyCollider.SetPath(0, newPoints);
    }
}
