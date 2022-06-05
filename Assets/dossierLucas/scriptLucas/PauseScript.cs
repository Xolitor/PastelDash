using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameController gameController;
    Text textePause;
    void Start()
    {
        textePause = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        textePause.enabled = !gameController.isGameRunning;
        // Affiche le texte seulement si le jeu est en pause
    }
}
