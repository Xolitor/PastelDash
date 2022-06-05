using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public bool isGameRunning; // indique si le jeu est en pause
    public bool isGravityUp; // indique si la gravité est vers le haut
    public bool isForceShieldOn; // indique si le bouclié est allumé

    public float forceShieldDuration; // durée d'utilisation
    public float forceShieldReload ; // durée de rechargement
    public float currentForceShield; //power restant en pourcent

    public enum StateOfGame // différents "état du jeu"
    {
        Normal, GameOver, Boss, Start, Error
    }
    public enum GameEvent // différents évennement qui peuvent survenir durant le jeu
    {
        BonusScore, EnnemyKill, GoUp, GoDown, ShieldActivation, LevelUp
    }
    public StateOfGame currentStateOfGame; // état courant du jeu
    public List<GameEvent> currentGameEvent = new List<GameEvent>(); // Liste de tous les events en cours
    //Note : les events sont supprimé par les autres scripts une fois qu'il ont pris l'évent en compte

    public int currentLevel; // niveau courant du jeu

    // Start is called before the first frame update
    void Start()
    {
        //Valeur par défaut
        forceShieldDuration = 1;
        forceShieldReload = 10;
        currentForceShield = 100;
        currentLevel = 0;

        currentStateOfGame = StateOfGame.Normal;
        isGameRunning = true;
        isGravityUp = false;
        isForceShieldOn = false; 
    }

    // Update is called once per frame
    void Update()
    {
        MainInput();
        ForceShieldVariateur();
    }
    /*private void StartStopInput() // Capture la mise en pause du jeu
    {

        isGameRunning = Input.GetKeyDown(KeyCode.P) ? !isGameRunning : isGameRunning;
        
    }*/
    private void GravityInput() // Capture le changement de gravité
    {
        if (this.isGameRunning)
        { // si ça joue :

            if (Input.GetKeyDown(KeyCode.Space)) // update isGravityUp et rajoute l'event de changement de gravité en conséquences
            {
                if (this.isGravityUp)
                {
                    this.currentGameEvent.Add(GameEvent.GoDown); 
                    this.isGravityUp = false;
                }
                else // et inversement
                {
                    this.isGravityUp = true;
                    this.currentGameEvent.Add(GameEvent.GoUp); 
                }
            }

        }
    }
    private void MainInput() // Gestion des différents input (touches utilisées temporaires)
    {
        //StartStopInput();
        GravityInput();
        ForceShiedInput();
    }
    private void ForceShiedInput() // capture l'activation du ForceShield
    {
        if (isGameRunning)
        {
            bool wasForceShieldOn = isForceShieldOn; // enregistre l'état précédent

            if (Input.GetKey(KeyCode.A) ) // empeche d'activé le ForceShield s'il n'est pas entièrement rechargé
            {
                if(currentForceShield == 100) { isForceShieldOn = true; }
                if(currentForceShield == 0) { isForceShieldOn = false; }
            }
            else { isForceShieldOn = false; } // désactivé par défaut

            if(!wasForceShieldOn && isForceShieldOn) { currentGameEvent.Add(GameEvent.ShieldActivation); } 
            // ajoute l'event ShieldActivation à la liste d'event
        }
    }
    private void ForceShieldVariateur() // Fait varier la puissance restante  du ForceShield
    {
        if (isGameRunning) {
            int max;
            float duration;
            float currentTime = Time.deltaTime;
            float facteur = currentForceShield / 100;

            if (isForceShieldOn) //decrease si actif
            {
                max = 0;
                duration = facteur * forceShieldDuration;
            }
            else //increase si inactif
            {
                max = 100;
                duration = forceShieldReload - facteur * forceShieldReload;
            }
            
            currentForceShield = (duration == 0) ? max : currentForceShield = Mathf.Lerp(currentForceShield, max, currentTime / duration);
            //évite la divison par zéro
        }
    }

}
