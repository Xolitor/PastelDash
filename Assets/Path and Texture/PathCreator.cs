using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    [HideInInspector]
    public Path path;

    public Color anchorCol = Color.red;
    public Color controlCol = Color.white;
    public Color segmentCol = Color.green;
    public Color selectedSegmentCol = Color.yellow;
    public float anchorDiameter = .1f;
    public float controlDiameter = 0.075f;
    public bool displayControlPoints = true;

    private Vector2 currentPos;

    public void CreatePath()
    {
        currentPos = transform.position;
        path = new Path(currentPos);
        transform.hasChanged = false;
    }
    private void Reset()
    {
        CreatePath();
    }
    private void Update()
    {
      
    }

}
