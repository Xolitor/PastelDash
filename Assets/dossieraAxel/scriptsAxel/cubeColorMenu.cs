using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeColorMenu : MonoBehaviour
{
    //Ce code est en partit celui de Lucas je l'ai adapte a mes besoins vis a vis du menu
    //Ce script se charge du changement de couleurs des cubes en arriere plan du menu d'acceuil
    //J'ai princiaplement modifie la fonction LerpColor qui affiche des changements de couleurs aleatoire differente de la version de Lucas qui s'aide de tableau predefinis de couleurs

    Renderer rend;
    public float duration = 5.0f; //temps d'attente entre chaque transition
    public float lerpDuration = 4.0f; //duree du lerp/transition d'une couleur a une autre
    public float smoothness = 0.02f;
    public StateOfGame currentStateOfGame;

    void Start()
    {
        rend = GetComponent<Renderer>();
        StartCoroutine(mainSwitch());
    }

    public enum StateOfGame
    {
        Normal = 0, GameOver = 1, Boss = 2, Start = 3, Error = 4 
    }

    public IEnumerator mainSwitch()
    {
        while (true)
            switch (this.currentStateOfGame)
            {
                case StateOfGame.Normal:
                    Debug.Log("mainSwitch : stateOfGame Normal");
                    StartCoroutine(LerpColor());
                    yield return new WaitForSeconds(duration);
                    break;
            }
    }
    IEnumerator LerpColor()
    {
        float progress = 0; 
        float increment = smoothness / lerpDuration; //la quantite de changement a appliquer 

        Color32 actualColor = rend.material.color; //on obtient la couleur du Game Object actuel
        Color32 nextColor = new Color32( (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(255, 256)); //on cree une autre couleur aleatoire

        while (progress < 1)
        {
            rend.material.color = Color.Lerp(actualColor, nextColor, progress); //la couleur de notre Game Object va lerp
            progress += increment; //le role de l'increment de tout a l'heure
            yield return new WaitForSeconds(smoothness);
        }
    }
}
