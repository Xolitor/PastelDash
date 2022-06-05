using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    public List<GameObject> pltPrefabs = new List<GameObject>();
    public List<GameObject> listOfInstances = new List<GameObject>();
    
    public int nbPlatformes = 5;
   
    public GameObject firstTerrain;

    public Vector2 lastPointOnThePath;

 
    // Start is called before the first frame update
    void Start()
    {
        //listOfInstances.Add(firstTerrain);
        lastPointOnThePath = firstTerrain.GetComponent<RoadCreator>().points[firstTerrain.GetComponent<RoadCreator>().points.Length-1];
        string path = "Prefabs/platformPrefabs/Platforms";
        for (int i = 1; i<= nbPlatformes; i++)
        {
            Debug.Log(i);
            GameObject prefab = Resources.Load(path + i.ToString(), typeof(GameObject)) as GameObject;
            pltPrefabs.Add(prefab);
        }
        
        for (int i = 0; i<= 2; i++)
        {
            renderNewTerrain((lastPointOnThePath + Vector2.down), pltPrefabs[Random.Range(0, pltPrefabs.Count)]);
        }
       
    }

    
    //GameObject newTerrain;
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GameObject.Find("Player").GetComponent<PlayerControl2>().currentPlatform.transform.parent.gameObject.name);
        //Debug.Log(GameObject.Find("Player").GetComponent<PlayerControl2>().currentPlatform.name);
        if (GameObject.Find("Player").GetComponent<PlayerControl2>().currentPlatform.transform.parent.gameObject == listOfInstances[2])
        {
            Destroy(listOfInstances[0]);
            renderNewTerrain((lastPointOnThePath + Vector2.down), pltPrefabs[Random.Range(0, pltPrefabs.Count)]);
            listOfInstances.RemoveAt(0);
            
        }

    }

    void renderNewTerrain(Vector2 startingPoint, GameObject terrain)
    {
        GameObject newTerrain = Instantiate(terrain);
        listOfInstances.Add(newTerrain);
        for (int i = 0; i < terrain.transform.childCount; i++)
        {
            newTerrain.transform.GetChild(i).GetComponent<RoadCreator>().path.RelocatePath(startingPoint);
            newTerrain.transform.GetChild(i).GetComponent<RoadCreator>().UpdateMesh();
        }

        int lastChildIndex = terrain.transform.childCount - 1;
        GameObject lastChild = newTerrain.transform.GetChild(terrain.transform.childCount - 1).gameObject;

        lastPointOnThePath = lastChild.GetComponent<RoadCreator>().points[lastChild.GetComponent<RoadCreator>().points.Length-1];
    }

}
