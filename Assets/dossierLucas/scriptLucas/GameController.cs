using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public bool isGameRunning; // indique si le jeu est en pause
    public bool isGravityUp; // indique si la gravit� est vers le haut
    public bool isForceShieldOn; // indique si le boucli� est allum�

    public float forceShieldDuration; // dur�e d'utilisation
    public float forceShieldReload ; // dur�e de rechargement
    public float currentForceShield; //power restant en pourcent

    public enum StateOfGame // diff�rents "�tat du jeu"
    {
        Normal, GameOver, Boss, Start, Error
    }
    public enum GameEvent // diff�rents �vennement qui peuvent survenir durant le jeu
    {
        BonusScore, EnnemyKill, GoUp, GoDown, ShieldActivation, LevelUp
    }
    public StateOfGame currentStateOfGame; // �tat courant du jeu
    public List<GameEvent> currentGameEvent = new List<GameEvent>(); // Liste de tous les events en cours
    //Note : les events sont supprim� par les autres scripts une fois qu'il ont pris l'�vent en compte

    public int currentLevel; // niveau courant du jeu

    // Start is called before the first frame update
    void Start()
    {
        //Valeur par d�faut
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
    private void GravityInput() // Capture le changement de gravit�
    {
        if (this.isGameRunning)
        { // si �a joue :

            if (Input.GetKeyDown(KeyCode.Space)) // update isGravityUp et rajoute l'event de changement de gravit� en cons�quences
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
    private void MainInput() // Gestion des diff�rents input (touches utilis�es temporaires)
    {
        //StartStopInput();
        GravityInput();
        ForceShiedInput();
    }
    private void ForceShiedInput() // capture l'activation du ForceShield
    {
        if (isGameRunning)
        {
            bool wasForceShieldOn = isForceShieldOn; // enregistre l'�tat pr�c�dent

            if (Input.GetKey(KeyCode.A) ) // empeche d'activ� le ForceShield s'il n'est pas enti�rement recharg�
            {
                if(currentForceShield == 100) { isForceShieldOn = true; }
                if(currentForceShield == 0) { isForceShieldOn = false; }
            }
            else { isForceShieldOn = false; } // d�sactiv� par d�faut

            if(!wasForceShieldOn && isForceShieldOn) { currentGameEvent.Add(GameEvent.ShieldActivation); } 
            // ajoute l'event ShieldActivation � la liste d'event
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
            //�vite la divison par z�ro
        }
    }

}
