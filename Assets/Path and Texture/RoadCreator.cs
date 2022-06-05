using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathCreator))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]


public class RoadCreator : MonoBehaviour
{
    [Range(.05f,1.5f)]
    public float spacing = 1;
    public float roadWidth = 3;
    public bool autoUpdate = true;
    public bool isGround = true;

    [HideInInspector]
    public Path path;
    [HideInInspector]
    public Vector2[] points;

    public void Start()
    {
        path = GetComponent<PathCreator>().path;
    }
    public void UpdateRoad()
    {
        path = GetComponent<PathCreator>().path;
        points = path.CalculateEvenlySpacedPoints(spacing);
        GetComponent<MeshFilter>().mesh = CreateRoadMesh(points, path.IsClosed);
        GetComponent<EdgeCollider2D>().points = points;
    }
    public void UpdateMesh()
    {
        points = path.CalculateEvenlySpacedPoints(spacing);
        GetComponent<MeshFilter>().mesh = CreateRoadMesh(points, path.IsClosed);
        GetComponent<EdgeCollider2D>().points = points;
    }

    Mesh CreateRoadMesh(Vector2[] points, bool isClosed)
    {
        Vector3[] verts = new Vector3[points.Length * 2];
        Vector2[] uvs = new Vector2[verts.Length];
        int numTris = 2 * (points.Length - 1) + ((isClosed) ? 2 : 0);

        int[] tris = new int[numTris * 3];
        int vertIndex = 0;
        int triIndex = 0;
        for (int i = 0; i < points.Length; i++)
        {
            Vector2 forward = Vector2.zero;
            if(i < points.Length - 1 || isClosed) 
            {
                forward += points[(i + 1)%points.Length] - points[i];
            }
            if (i > 0 || isClosed)
            {
                forward += points[i] - points[(i - 1 + points.Length)%points.Length];
            }
            forward.Normalize();
            Vector2 left = new Vector2(-forward.y, forward.x);


            if (isGround) {
                verts[vertIndex] = points[i] - left * (GameObject.Find("Player").transform.localScale.y / 2);
                verts[vertIndex + 1] = points[i] - left * roadWidth * .5f;
            }
            else
            {
                verts[vertIndex] = points[i] + left * roadWidth * .5f ;
                verts[vertIndex + 1] = points[i] + left * (GameObject.Find("Player").transform.localScale.y / 2);
            }
            

            float completionPercent = i / (float)(points.Length - 1);
            uvs[vertIndex] = new Vector2(0, completionPercent);
            uvs[vertIndex + 1] = new Vector2(1, completionPercent);

            if (i<points.Length - 1 || isClosed)
            {
                tris[triIndex] = vertIndex;
                tris[triIndex + 1] = (vertIndex + 2)%verts.Length;
                tris[triIndex + 2] = vertIndex + 1;

                tris[triIndex+3] = vertIndex +1;
                tris[triIndex + 4] = (vertIndex + 2)% verts.Length;
                tris[triIndex + 5] = (vertIndex + 3)% verts.Length;
            }

            vertIndex += 2;
            triIndex += 6;
        }
        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        return mesh;
    }

    public List<Vector2> roadPoints()
    {
        float lengthAdded = GameObject.Find("Player").transform.localScale.x / 2;
        List<Vector2> roadPts = new List<Vector2>(points);
        roadPts.Add(roadPts[roadPts.Count - 1] + (roadPts[roadPts.Count-1] - roadPts[roadPts.Count-2]).normalized * lengthAdded);
        return (roadPts);
        
    }
    public List<Vector2> roadPointsMesh()
    {
        List<Vector2> roadPts = new List<Vector2>();
        Vector3[] verts = GetComponent<MeshFilter>().mesh.vertices;
        for (int i = 0; i< verts.Length; i++)
        {
            if(i%2 == 0)
            {
                roadPts.Add(verts[i]);
            }
        }
        float lengthAdded = GameObject.Find("Player").transform.localScale.x / 2;
        roadPts.Add(roadPts[roadPts.Count - 1] + (roadPts[roadPts.Count - 1] - roadPts[roadPts.Count - 2]).normalized * lengthAdded);
        return (roadPts);

    }
}
