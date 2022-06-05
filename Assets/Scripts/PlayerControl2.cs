using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerControl2 : MonoBehaviour
{
    public GameObject startingGround;

    public float spacing;
    public float speed = 1f;
    public float speedtest = 5f;

    public float inAirDuration = 0f;


    public bool onGround = true;  //Variable qui va d�terminer si le Cube touche actuellement une plateforme.
    [HideInInspector]
    public bool groundGliding = true; //Variable mal nomm�e, permettant de d�terminer si le Cube est sur la plateforme du haut ou du bas
    [HideInInspector]
    public Vector3 currentMovement;

    private int indexPos = 0;


    [HideInInspector]
    List<Vector2> currentPoints;
    public GameObject currentPlatform;

    private Coroutine movingCube;
    private Coroutine rotateCube;
    private Coroutine jumpCube;
    private Coroutine fall;

    Quaternion applyRotation;

   
    void Start()  // Start is called before the first frame update
    {

        currentPlatform = startingGround;
        currentPoints = startingGround.GetComponent<RoadCreator>().roadPoints(); //On r�cup�re les points qui forment la plateforme de d�part
        spacing = startingGround.GetComponent<RoadCreator>().spacing; //On r�cup�re un param�tre qui nous aidera � d�placer le cube

        movingCube = StartCoroutine(MoveObject(currentPoints, currentPoints[2])); //On lance 2 coroutines,c'est un type de fonction qui continue de s'ex�cuter d'une frame � l'autre
        rotateCube = StartCoroutine(RotateObject(currentPoints, currentPoints[2])); //On les utilise pour diriger le cube, afin qu'il donne l'impression de glisser sur le terrain
    }

    
    void Update() // Update is called once per frame
    {
        if(inAirDuration> 2f)
        {
            StartCoroutine(deathanim());
        }
        if (indexPos >= currentPoints.Count && onGround) //Ici on v�rifie que le Cube est encore sur une plateforme, et n'a pas atteint le bout.
        {
            
            StopCoroutine(movingCube);                  
            StopCoroutine(rotateCube);
            fall = StartCoroutine(Fall(currentPoints));// Sinon, on met fin aux Coroutines qui g�raient le d�placement du Cube, et on lance une Coroutine qui g�re sa chute
            onGround = false;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && onGround)//Ici on regarde si le joueur a appuy� sur la barre espace, et qu'il �tait bien sur une plateforme
        {
            StopCoroutine(movingCube);
            StopCoroutine(rotateCube);
            jumpCube = StartCoroutine(Jump(currentPoints)); //On arr�te les autres Coroutine et on lance celle qui g�re le saut
            onGround = false;
        }
    }

    public IEnumerator MoveObject(List<Vector2> posList, Vector2 startPos) //Fonction qui va permettre de d�placer le Cube en g�rant la composante "position" du GameObject
    {
        indexPos = posList.IndexOf(startPos);

        Vector2 currentPos = posList[indexPos];
        Vector2 nextPos = posList[indexPos + 1];
        float currentMovementTime;//The amount of time that has passed

        while (true) //Here we allow ourself to have an infinite loop as we will kill the process with outside functions
        {
            //in this infinite loop we will go through every points that make the path of the road
            float totalMovementTime = spacing / speed;

            currentMovementTime = 0f;
            while (Vector3.Distance(transform.localPosition, nextPos) > 0) //this smaller While loop is the one that's in charge of moving smoothly the Cube, in a continuous manner between the points forming te path; 
            {
                currentMovementTime += Time.deltaTime;
                currentMovement = Vector3.Lerp(currentPos, nextPos, currentMovementTime / totalMovementTime);// The Lerp function is short for Linear Interpolation, and it allows us to compute a the position that the Cube should have at a given time;
                this.transform.localPosition = currentMovement;//here we apply the new position to the Cube;
                yield return null;//this line stops the Coroutine from executing for this frame;
            }
            indexPos += 1;
            currentPos = nextPos;

            if (indexPos < posList.Count)
            {
                nextPos = posList[indexPos]; //Once we've shifted the Cube's position to a new point on the path, we change it's next target position to the next point on the path
            }

            yield return null;
        }
    }


    public IEnumerator RotateObject(List<Vector2> posList, Vector2 startPos)//the coroutine that will rotate the Cube according to how the path will curve
    {
        int indexPos = posList.IndexOf(startPos);
        Vector2 currentPos = posList[indexPos];
        Vector2 nextPos = posList[indexPos + 1];
        Vector2 vecTo = (nextPos - currentPos);

        float currentAngle = Vector2.SignedAngle(Vector2.right, vecTo);
        ApplyRotation(currentAngle);

        float nextAngle = currentAngle;
        float currentMovementTime;

        while (true) //We are doing the same thing as the MoveObject Coroutine, but with the rotation parameter of the
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
            if (indexPos < posList.Count - 1)
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
        Vector3 angularVector = new Vector3(0f, 0f, rotationZ); //Vecteur n�cessaire pour l'injecter dans le Quaternion rotation
        applyRotation.eulerAngles = angularVector; //Cr�ation du Quaternion pour le GameObject
        this.transform.rotation = applyRotation;
    }

    public IEnumerator Jump(List<Vector2> currentPlatform)
    {
        while (true)
        {
            inAirDuration += Time.deltaTime;
            Vector3 vecteurNormal;
            if (groundGliding)
            {
                vecteurNormal = (Vector2.Perpendicular(currentPlatform[indexPos + 1] - currentPlatform[indexPos]).normalized) + (currentPlatform[indexPos + 1] - currentPlatform[indexPos]).normalized;

            }
            else
            {
                vecteurNormal = Vector2.Perpendicular(currentPlatform[indexPos] - currentPlatform[indexPos + 1]).normalized + (currentPlatform[indexPos + 1] - currentPlatform[indexPos]).normalized;
            }
            this.transform.position += vecteurNormal * Time.deltaTime * speedtest;
            yield return null;
        }
    }

    public IEnumerator Fall(List<Vector2> currentPlatform)
    {
        inAirDuration += Time.deltaTime;
        StopCoroutine(rotateCube);
        StopCoroutine(movingCube);
        int lastIndex = currentPlatform.Count - 1;
        while (true)
        {
            Vector3 vecteurNormal;
            if (!groundGliding)
            {
                vecteurNormal = ((Vector2.Perpendicular(currentPlatform[lastIndex] - currentPlatform[lastIndex - 1]).normalized) + (currentPlatform[lastIndex] - currentPlatform[lastIndex - 1]).normalized).normalized;

            }
            else
            {
                vecteurNormal = (Vector2.Perpendicular(currentPlatform[lastIndex - 1] - currentPlatform[lastIndex]).normalized + (currentPlatform[lastIndex] - currentPlatform[lastIndex - 1]).normalized).normalized;
            }
            this.transform.position += vecteurNormal * Time.deltaTime * speedtest;
            yield return null;
        }
    }


   IEnumerator deathanim()
    {
        SceneManager.LoadScene("DeathScreen");
        yield return new WaitForSeconds(0.5f);
        
    }

    private UtilityScripts Utility = new UtilityScripts();
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.collider.gameObject;
        if (collidedObject.layer == 3 && !onGround && !collidedObject.name.Equals(collision.otherCollider.gameObject.name))
        {
            Vector2 currentPos = transform.position;
            List<Vector2> road = collidedObject.GetComponent<RoadCreator>().roadPoints();
            currentPlatform = collidedObject;
            currentPoints = road;
            spacing = startingGround.GetComponent<RoadCreator>().spacing;
            Vector2 newPos = Utility.closestPoint(road, currentPos);
            StopAllCoroutines();
            transform.position = newPos;
            movingCube = StartCoroutine(MoveObject(road, newPos));
            rotateCube = StartCoroutine(RotateObject(road, newPos));
            onGround = !onGround;
            groundGliding = collidedObject.GetComponent<RoadCreator>().isGround;
            inAirDuration = 0;
        }
    }


}
