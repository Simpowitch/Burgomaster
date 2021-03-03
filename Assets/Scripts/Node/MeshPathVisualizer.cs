using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class MeshPathVisualizer : PathVisualizer
{
    MeshFilter meshFilter;

    public override void Setup(Vector2[] points, bool isClosed, float pathWidth)
    {
        if (meshFilter == null)
            meshFilter = GetComponent<MeshFilter>();

        meshFilter.mesh = CreatePathMesh(points, isClosed, pathWidth);
    }

    Mesh CreatePathMesh(Vector2[] points, bool isClosed, float pathWidth)
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

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(verts[i].x, verts[i].y);
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;

        return mesh;
    }
}
