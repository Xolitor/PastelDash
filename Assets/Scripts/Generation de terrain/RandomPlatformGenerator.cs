using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlatformGenerator
{

    public int lowerPlatformLengthLimit = 6;
    public int upperPlatformLengthLimit = 16;

    public float lowerLengthLimit = 3;
    public float upperLengthLimit = 9;
    public float angleLimit = 0.523599f;//en radian

    private UtilityScripts uti = new UtilityScripts();
    private Path path;
    public GameObject CreatePlatform(bool isGround,Material terrainTexture, Vector2 startingPoint, Vector2 vecDirection)
    {
        GameObject newPlatform = new GameObject("Platform");
        newPlatform.transform.position = Vector3.zero;
        newPlatform.layer = 3; //layer 3 = Terrain
        newPlatform.AddComponent<PathCreator>();
        newPlatform.AddComponent<RoadCreator>();
        newPlatform.GetComponent<MeshRenderer>().material = terrainTexture;
        newPlatform.GetComponent<RoadCreator>().isGround = isGround;

        List<Vector2> rdPath = randomPath((Random.Range(lowerPlatformLengthLimit, upperPlatformLengthLimit)), vecDirection,startingPoint); //


        newPlatform.GetComponent<PathCreator>().path = new Path(rdPath[0], rdPath[1]);
        for (int i = 2; i< rdPath.Count; i++)
        {
            newPlatform.GetComponent<PathCreator>().path.AddSegment(rdPath[i]);
        }
        newPlatform.GetComponent<PathCreator>().path.AutoSetControlPoints = true;

        newPlatform.GetComponent<RoadCreator>().UpdateRoad();
        return newPlatform;
    }


    public List<Vector2> randomPath(int length, Vector2 vecDirection, Vector2 startingPoint)
    {
        List<Vector2> rdPoints = new List<Vector2>();
        rdPoints.Add(startingPoint);
        rdPoints.Add(nextRdPoint(vecDirection, startingPoint));
        for (int i = 2; i< length; i++)
        {
            rdPoints.Add(nextRdPoint((rdPoints[i - 1]- rdPoints[i - 2]).normalized, rdPoints[i-1]));
        }
        return rdPoints;
    }

    public Vector2 nextRdPoint(Vector2 vecDirection, Vector2 lastPoint)
    {
        int[] upDown = new int[2] { -1, 1 };
        int upOrDown = upDown[Random.Range(0, 2)];
        float rdAngle = Random.Range(0f, angleLimit)*upOrDown;
        
        Vector2 rotated = uti.rotateVector(rdAngle,vecDirection);
        Vector2 nextPoint = lastPoint + (rotated * (Random.Range(lowerLengthLimit, upperLengthLimit)));
        return nextPoint;
    }

    
}
