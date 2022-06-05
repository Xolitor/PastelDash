using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpScript : MonoBehaviour // Gestion de l'affichage du Niveau
{
    public GameController gameController;
    Text textLevelUp;
    public float duration;
    // Start is called before the first frame update
    void Start()
    {
        duration = 3;
        textLevelUp = GetComponent<Text>();
        textLevelUp.enabled = false;
        StartCoroutine(Affichage());
    }

    private IEnumerator Affichage() //si nécessaire, affiche le nouveau niveau atteint pendant 3 secs
    {
        while (true)
        {
            if (gameController.currentGameEvent.Contains(GameController.GameEvent.LevelUp))
                //si event LevelUp, on update l'affichage, puis on suppr l'event
            {
                textLevelUp.enabled = true;
                textLevelUp.text = "Level Up ! : " + gameController.currentLevel.ToString();
                gameController.currentGameEvent.Remove(GameController.GameEvent.LevelUp);
                yield return new WaitForSeconds(duration); // durée en seconds d'affichage
                textLevelUp.enabled = false;
            }
            yield return null;
        }
    }
    
}
