using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject startingGround;

    public  float spacing = 1;
    public  float resolution = 1;
    public  float speed = 1f;
    public float speedtest = 5f;

    [HideInInspector]
    public bool onGround = true;
    [HideInInspector]
    public bool groundGliding = true;
    [HideInInspector]
    public Vector3 currentMovement;

    private int indexPos = 0;


    [HideInInspector]
    Vector2[] currentPoints;

    private Coroutine movingCube;
    private Coroutine rotateCube;
    private Coroutine jumpCube;
    private Coroutine fall;

    Quaternion applyRotation;

    // Start is called before the first frame update
    void Start()
    {
        Path pathGround = startingGround.GetComponent<RoadCreator>().path;

        currentPoints = pathGround.CalculateEvenlySpacedPoints(spacing,resolution);


        //movingCube = StartCoroutine(MoveObject(currentPoints, currentPoints[0]));
        //rotateCube = StartCoroutine(RotateObject(currentPoints, currentPoints[0]));
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = currentPoints[0];

        if (indexPos >= currentPoints.Length && onGround)
        {
            StopCoroutine(movingCube);
            StopCoroutine(rotateCube);
            fall = StartCoroutine(Fall(currentPoints));
            onGround = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            StopCoroutine(movingCube);
            StopCoroutine(rotateCube);
            jumpCube = StartCoroutine(Jump(currentPoints));
            onGround = false;
        }
    }

    public IEnumerator MoveObject(Vector2[] posList , Vector2 startPos)
    {
        indexPos = System.Array.IndexOf(posList, startPos);
        Vector2 currentPos = posList[indexPos];
        Vector2 nextPos = posList[indexPos + 1];
        float currentMovementTime;//The amount of time that has passed


        while (true)
        {

            float totalMovementTime = spacing / speed;
           
            currentMovementTime = 0f;
            while (Vector3.Distance(transform.localPosition, nextPos) > 0)
            {
                currentMovementTime += Time.deltaTime;
                currentMovement = Vector3.Lerp(currentPos, nextPos, currentMovementTime / totalMovementTime);
                this.transform.localPosition = currentMovement;
                yield return null;
            }
            indexPos += 1;
            currentPos = nextPos;
            
            if (indexPos < posList.Length)
            {
                nextPos = posList[indexPos];
            }
            
            yield return null;
        }


    }
    public IEnumerator RotateObject(Vector2[] posList , Vector2 startPos)
    {
        int indexPos = System.Array.IndexOf(posList, startPos);
        Vector2 currentPos = posList[indexPos];
        Vector2 nextPos = posList[indexPos + 1];
        Vector2 vecTo = (nextPos - currentPos);

        float currentAngle = Vector2.SignedAngle(Vector2.right, vecTo);
        ApplyRotation(currentAngle);

        float nextAngle = currentAngle;
        float currentMovementTime;

        while (true)
        {
            float totalMovementTime = spacing / speed;
            currentMovementTime = 0f;
            while (Vector3.Distance(transform.localPosition, nextPos) > Vector3.Distance(currentPos, nextPos) / 2)
            {
                currentMovementTime += Time.deltaTime;
                ApplyRotation(Mathf.Lerp(currentAngle, nextAngle, currentMovementTime / totalMovementTime));
                yield return null;
            }
            indexPos += 1;
            currentPos = nextPos;
            if (indexPos < posList.Length-1)
            {
                nextPos = posList[indexPos];

                currentAngle = nextAngle;
                vecTo = (posList[indexPos + 1] - posList[indexPos]);
                nextAngle = Vector2.SignedAngle(Vector2.right, vecTo);
            }
           

            yield return null;
        }
    }

    public void ApplyRotation(float rotationZ)
    {
        Vector3 angularVector = new Vector3(0f, 0f, rotationZ); //Vecteur nécessaire pour l'injecter dans le Quaternion rotation
        applyRotation.eulerAngles = angularVector; //Création du Quaternion pour le GameObject
        this.transform.rotation = applyRotation;
    }

    public IEnumerator Jump(Vector2[] currentPlatform)
    {
        while (true)
        {
            Vector3 vecteurNormal;
            if (groundGliding)
            {
                vecteurNormal = (Vector2.Perpendicular(currentPlatform[indexPos + 1] - currentPlatform[indexPos]).normalized) + (currentPlatform[indexPos + 1] - currentPlatform[indexPos]).normalized;
              }
            else
            {
                vecteurNormal = Vector2.Perpendicular(currentPlatform[indexPos] - currentPlatform[indexPos + 1]).normalized + (currentPlatform[indexPos + 1] - currentPlatform[indexPos]).normalized; 
            }
            this.transform.position += vecteurNormal*Time.deltaTime*speedtest;
            yield return null;
        }
    }

    public IEnumerator Fall(Vector2[] currentPlatform)
    {
        StopCoroutine(rotateCube);
        StopCoroutine(movingCube);
        int lastIndex = currentPlatform.Length-1;
        while (true)
        {
            Vector3 vecteurNormal;
            if (!groundGliding)
            {
                vecteurNormal = ((Vector2.Perpendicular(currentPlatform[lastIndex] - currentPlatform[lastIndex-1]).normalized) + (currentPlatform[lastIndex] - currentPlatform[lastIndex - 1]).normalized).normalized;

            }
            else
            {
                vecteurNormal = (Vector2.Perpendicular(currentPlatform[lastIndex - 1] - currentPlatform[lastIndex]).normalized + (currentPlatform[lastIndex] - currentPlatform[lastIndex - 1]).normalized).normalized;
            }
            this.transform.position += vecteurNormal * Time.deltaTime * speedtest;
            yield return null;
        }
    }


    private UtilityScripts Utility = new UtilityScripts();
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.collider.gameObject;
        if (collidedObject.layer==3 && !onGround && !collidedObject.name.Equals(collision.otherCollider.gameObject.name))
        {
            Vector2 currentPos = transform.position;
            Vector2[] road = collidedObject.GetComponent<PathCreator>().path.CalculateEvenlySpacedPoints(spacing, resolution);
            currentPoints = road;
            Vector2 newPos = Utility.closestPoint(road, currentPos);
            StopAllCoroutines();

            transform.position = newPos;
            onGround = true;
            groundGliding = collidedObject.GetComponent<RoadCreator>().isGround;
            
            movingCube = StartCoroutine(MoveObject(currentPoints, newPos));
            rotateCube = StartCoroutine(RotateObject(currentPoints, newPos));
            
        }
    }


}
