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

        //meshFilter.mesh = CreatePolyMesh(width / 2, circleVertices);

        meshFilter.mesh = CreateCircleMesh(width / 2, circleVertices);

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
        Vector2[] uv2s = new Vector2[verts.Length];

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
        //mesh.uv2 =

        return mesh;
    }

    Mesh CreateCircleMesh(float radius, int n)
    {
        Mesh mesh = new Mesh();

        int vertexCount = n + 1;
        Vector3[] verts = new Vector3[vertexCount];
        int[] tris = new int[(vertexCount - 2) * 3];

        Vector2[] uvs = new Vector2[verts.Length];
        Vector2[] uv2s = new Vector2[verts.Length];

        verts[0] = Vector2.zero;
        uv2s[0] = new Vector2(1, 0);

        tris[0] = 0;
        tris[1] = 1;
        tris[2] = 2;

        float angleInDegrees = 0;
        float angleChange = 360f / (n -1);

        for (int i = 1; i < vertexCount; i++)
        {
            Vector3 offset = Utility.DirFromAngle(angleInDegrees) * radius;
            verts[i] = verts[0] + offset;
            uv2s[i] = new Vector2(0, 0);

            angleInDegrees += angleChange;

            if (i < vertexCount - 2)
            {
                tris[i * 3] = 0;
                tris[i * 3 + 1] = i + 1;
                tris[i * 3 + 2] = i + 2;
            }
        }

        for (int i = 0; i < uvs.Length; i++)
        {
            Vector2 worldPos = this.transform.position;
            uvs[i] = new Vector2(verts[i].x + worldPos.x, verts[i].y + worldPos.y);
        }

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.uv2 = uv2s;

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
