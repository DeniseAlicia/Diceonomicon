using UnityEngine;
using System.Collections.Generic;

public static class TubeMeshBuilder
{
    public static Mesh BuildTube(Vector3[] points, float radius, int radialSegments)
    {
        Mesh mesh = new Mesh();
        if (points == null || points.Length < 2) return mesh;

        int rings = points.Length;
        int sides = Mathf.Max(3, radialSegments);

        Vector3[] vertices = new Vector3[rings * sides];
        Vector3[] normals = new Vector3[rings * sides];
        List<int> tris = new List<int>((rings - 1) * sides * 6);

        for (int i = 0; i < rings; i++)
        {
            Vector3 forward = (i < rings - 1) ? (points[i + 1] - points[i]) : (points[i] - points[i - 1]);
            forward.Normalize();

            Vector3 up = Vector3.up;
            if (Mathf.Abs(Vector3.Dot(forward, up)) > 0.9f)
                up = Vector3.right;

            Vector3 right = Vector3.Cross(up, forward).normalized;
            up = Vector3.Cross(forward, right).normalized;

            for (int s = 0; s < sides; s++)
            {
                float angle = Mathf.PI * 2f * s / sides;
                Vector3 offset = right * Mathf.Cos(angle) * radius + up * Mathf.Sin(angle) * radius;
                int idx = i * sides + s;
                vertices[idx] = points[i] + offset;
                normals[idx] = offset.normalized;
            }
        }

        for (int i = 0; i < rings - 1; i++)
        {
            for (int s = 0; s < sides; s++)
            {
                int a = i * sides + s;
                int b = (i + 1) * sides + s;
                int c = i * sides + (s + 1) % sides;
                int d = (i + 1) * sides + (s + 1) % sides;

                tris.Add(a); tris.Add(b); tris.Add(c);
                tris.Add(c); tris.Add(b); tris.Add(d);
            }
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.triangles = tris.ToArray();
        mesh.RecalculateBounds();
        return mesh;
    }
}
