using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class pauseMenu : MonoBehaviour
{

    public GameObject bigPanel;
    public GameObject settingwindow;
    public GameController gameController;
    public Button paused;
    public AudioSource playsound;
    private void Update()
    {
        if (Input.GetKeyDown("escape")) //lorsqu'on appuie sur esc le jeu pause
        {
            pause();
        }
        /*if (!gameController.isGameRunning)
        {
            pause();
        }*/
    }

    public void pause()
    {
        playsound.Play();
        bigPanel.SetActive(true); //on active le menu de pause
        Time.timeScale = 0;
        gameController.isGameRunning = false;
    }

    public void Play()
    {
        playsound.Play();
        bigPanel.SetActive(false); //on desactive pr retourner au jeu
        Time.timeScale = 1;
        gameController.isGameRunning = true;
    }
    public void Settings()
    {
        playsound.Play();
        bigPanel.SetActive(false); //on desactive pr activer a la place le menu de reglage identique a celui du menu d'acceuil
        settingwindow.SetActive(true);
    }

    public void Exit()
    {
        playsound.Play();
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    public void closeSettings()
    {
        playsound.Play();
        bigPanel.SetActive(true);
        settingwindow.SetActive(false);
    }
}
