using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{

    public float x = 0;
    public float y = 0;

    public Material terrainTexture;
    public GameObject firstTerrain;
    
    private RandomPlatformGenerator gen = new RandomPlatformGenerator();
    void Start()
    {
        Vector2[] lastPoints = firstTerrain.GetComponent<RoadCreator>().points;
        int length = lastPoints.Length;
        GameObject newRoad = new GameObject();
        newRoad = gen.CreatePlatform(true, terrainTexture,lastPoints[length-1],(lastPoints[length - 1]- lastPoints[length - 2]).normalized);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
