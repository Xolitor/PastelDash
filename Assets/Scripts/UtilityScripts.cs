using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityScripts
{
    public Vector2 closestPoint(Vector2[] points, Vector2 origine)
    {
        float min = Vector2.Distance(points[0], origine);
        int iMin = 0;
        for (int i = 1; i < points.Length; i++)
        {
            float newMin = Vector2.Distance(points[i], origine);
            if (min > newMin)
            {
                iMin = i;
                min = newMin;
            }
        }
        return points[iMin];
    }

    public Vector2 closestPoint(List<Vector2> points, Vector2 origine)
    {
        float min = Vector2.Distance(points[0], origine);
        int iMin = 0;
        for (int i = 1; i < points.Count; i++)
        {
            float newMin = Vector2.Distance(points[i], origine);
            if (min > newMin)
            {
                iMin = i;
                min = newMin;
            }
        }
        return points[iMin];
    }

    public Vector2 rotateVector(float teta, Vector2 vec)
    {
        return new Vector2((Mathf.Cos(teta) * vec.x - Mathf.Sin(teta) * vec.y), (Mathf.Sin(teta) * vec.x + Mathf.Cos(teta) * vec.y));
    }
}