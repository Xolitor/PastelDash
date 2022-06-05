using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeMenuController : MonoBehaviour
{

    //Ce code est en partit celui de Garbis je l'ai adapte a mes besoins vis a vis du menu
    //Ce script code les deplacements des cubes vu en arriere plan par le joueur lorsqu'il est dans le menu d'acceuil
    //J'ai rajoute du code pour que les carres qui suivent la courbe puisse recommencer au debut lorsqu'il termine leurs trajectoires

    public GameObject ground;

    private Path pathGround;

    Vector2[] posGround;

    public float spacing = 1;
    public float resolution = 1;
    public float speed = 1;
    public float speedMin;
    public float speedMax;
    public float speedtest = 5f;


    public int indexPos = 0;

    private Coroutine movingCube;
    private Coroutine rotateCube;

    public SpriteRenderer rend;

    Quaternion applyRotation;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        pathGround = ground.GetComponent<PathCreator>().path;
        posGround = pathGround.CalculateEvenlySpacedPoints(spacing, resolution);


        movingCube = StartCoroutine(MoveObject(posGround));
        rotateCube = StartCoroutine(RotateObject(posGround));
    }


    public IEnumerator MoveObject(Vector2[] posList)
    {
        Vector2 startLocation = posGround[0]; //j'ai ajoute cette variable qui correspond au premier point de la courbe que va suivre le carre
        Vector2 currentPos = posList[indexPos];
        Vector2 nextPos = posList[indexPos + 1];
        float currentMovementTime;
        //Debug.Log(posList.Length);

        while (true)
        {
            //Debug.Log(speed);

            float totalMovementTime = spacing / speed;
            currentMovementTime = 0f;


            while (Vector3.Distance(transform.localPosition, nextPos) > 0)
            {
                currentMovementTime += Time.deltaTime;
                this.transform.localPosition = Vector3.Lerp(currentPos, nextPos, currentMovementTime / totalMovementTime);
                yield return null;
            }
            indexPos += 1;
            currentPos = nextPos;
            nextPos = posList[indexPos];
            //Debug.Log("currentPos" + currentPos);
            if (currentPos == posList[posList.Length-2]) //lorsque le carre arrive a la fin de la courbe
            {
                rend.enabled = false; //on le desactive
                this.transform.position = startLocation; // on le repositionne au debut 
                indexPos = 0; //on renitialise la variable index qui parcours notre courbe
            }
            if(currentPos == posList[1]) //une fois de nouveau partit depuis le debut
            {
                rend.enabled = true; //on le reactive aux yeux des joeurs
                speed = Random.Range(speedMin, speedMax); //avec une vitesse differente pr cree un peu de changement

            }

            yield return null;
        }

    }

    public IEnumerator RotateObject(Vector2[] posList)
    {
        int indexPos = 0;

        Vector2 startLocation = posGround[0];
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
            nextPos = posList[indexPos];
            if (currentPos == posList[posList.Length - 2]) //meme code que en haut mais pour la rotation du cube cette fois ci
            {
                this.transform.position = startLocation;
                indexPos = 0;
            }

            if (currentPos == posList[1])
            {
                speed = Random.Range(speedMin, speedMax);
            }
            currentAngle = nextAngle;
            vecTo = (posList[indexPos + 1] - posList[indexPos]);
            nextAngle = Vector2.SignedAngle(Vector2.right, vecTo);

            yield return null;
        }
    }

    public void ApplyRotation(float rotationZ)
    {
        Vector3 vecteurAngulaire = new Vector3(0f, 0f, rotationZ); //Vecteur ncessaire pour l'injecter dans le Quaternion rotation
        applyRotation.eulerAngles = vecteurAngulaire; //Cration du Quaternion pour le GameObject
        this.transform.rotation = applyRotation;
    }

}


