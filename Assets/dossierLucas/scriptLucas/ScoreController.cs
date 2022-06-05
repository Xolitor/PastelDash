using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public float currentScore = 0;
    public int gainByuptade = 1; // Bonus score par unité de temps
    public int bonusScore = 0; // Score gagné (ou perdu) recup d'autre file (Boss / obstacle / ennemies /items)
    public float deltaUpdate = 1.0f;
    public int scoreForLevelUp = 50;

    public GameController gameController; //script du gameController

    public Text score;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        score.text = " 0";
        player = GameObject.Find("Square");
        InvokeRepeating(nameof(UpdateScoreValue), 0, deltaUpdate); //toutes les deltaUpdate secondes, on update le score
    }


    private void UpdateScoreValue() // Update les différentes values associées aux score
    {
        if (gameController.isGameRunning)
        {
            //update de la valeur du score
            currentScore += gainByuptade + bonusScore;
            bonusScore = 0;
            score.text = " " + ((int)currentScore).ToString();
            
            if(currentScore>0 && currentScore % scoreForLevelUp == 0) // Si on passe le cap de score requis, levelUp
            {
                //update du niveau atteint
                gameController.currentLevel++;
                gameController.currentGameEvent.Add(GameController.GameEvent.LevelUp);
                //Debug.Log("Level Up : " + gameController.currentLevel);
            }
        }
        //sinon, on touche pas au score
    }
}
