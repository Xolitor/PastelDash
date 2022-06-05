using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public GameController gameController;

    public AudioSource startSound;
    public AudioSource bossSound;
    public AudioSource endSound;

    public AudioSource goUp;
    public AudioSource goDown;
    public AudioSource ennemyKill;
    public AudioSource bonusScore;
    public AudioSource fsActivation;

    private AudioSource currentSound;
    private AudioSource currentLoop;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if((startSound == null ) || (bossSound == null ) || (endSound == null) 
            || (goUp == null) || (goDown == null) || (ennemyKill == null) ||(bonusScore == null))
        {
            Debug.LogWarning("One audio source is null");
            // Avertissemnt s'il manque un son
        }
        else
        {
            startSound.Play();
            currentSound = startSound;
        }
        bossSound.loop = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.isGameRunning)
        {
            SwitchStateOfGame();
            SwitchGameEvent();
        }
        else
        {
            StopCurrentLoop();
        }
    }

    private void PlaySoundLoop(AudioSource sound) // Joue un son en boucle
    {
        if (currentLoop != sound)
        {
            if (!sound.loop)
            {
                Debug.LogWarning("Sound " + sound + "is not a loop");
            }
            //Debug.Log("playSoundLoop " + sound);
            sound.Play();
            currentLoop = sound;
        }
    }

    private void PlaySoundOne(AudioSource sound) // Permet de jouer qu'une fois un son malgrès un état constant
    {
        if (currentSound != sound)
        {
            //Debug.Log("playSound " + sound);
            sound.Play();
        }
    }

    private void StopCurrentLoop() // Arrête la boucle de son en cours
    {
        if (currentLoop != null)
        {
            currentLoop.Stop();
            currentLoop = null;
        }
    }
    private void SwitchStateOfGame() //Son joué selon l'état du jeu en cours
                                     //NOTES : Pourrait être remplacer par la création d'event de changement d'état du jeu
    {
        switch (gameController.currentStateOfGame)
        {
            case GameController.StateOfGame.GameOver:
                StopCurrentLoop();
                PlaySoundOne(endSound);
                currentSound = endSound;
                break;

            case GameController.StateOfGame.Boss:
                PlaySoundLoop(bossSound);
                currentSound = bossSound;
                break;

            default:
                StopCurrentLoop();
                currentSound = null;
                break;
        }
    }
    private void SwitchGameEvent() // Son Joué selon les différents évents présent
    {
        foreach (GameController.GameEvent e in gameController.currentGameEvent)
        {
            switch (e)
            {
                case GameController.GameEvent.GoDown:
                    this.goDown.Play();
                    break;

                case GameController.GameEvent.GoUp:
                    this.goUp.Play();
                    break;

                case GameController.GameEvent.EnnemyKill:
                    this.ennemyKill.Play();
                    break;

                case GameController.GameEvent.BonusScore:
                    this.bonusScore.Play();
                    break;

                case GameController.GameEvent.ShieldActivation:
                    this.fsActivation.Play();
                    break;
            }
        }
        //Clear après la boucle sinon, erreur car modification de liste utilisé
        //DO NOT USE CLEAR : pourrait suppr d'autres events
        gameController.currentGameEvent.RemoveAll(e => e.Equals(GameController.GameEvent.GoDown));
        gameController.currentGameEvent.RemoveAll(e => e.Equals(GameController.GameEvent.GoUp));
        gameController.currentGameEvent.RemoveAll(e => e.Equals(GameController.GameEvent.EnnemyKill));
        gameController.currentGameEvent.RemoveAll(e => e.Equals(GameController.GameEvent.BonusScore));
        gameController.currentGameEvent.RemoveAll(e => e.Equals(GameController.GameEvent.ShieldActivation));


    }

}
