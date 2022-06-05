using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class mainMenu : MonoBehaviour
{

    public GameObject settingwindow; //ref pour le menu des reglages ainsi que les differents bouttons qui s'affichent dans le menu principal
    public GameObject playButton;
    public GameObject exitButton;
    public GameObject creditButton;
    public AudioSource playsound; //ref a la piste audio dedie au bruit des boutons

    public void PlayButton()
    {
        playsound.Play(); //jouer la piste audio
        StartCoroutine(loadLevel()); //une coroutine qui permet d'attendre quelque seconde avant le chargement ici du niv
        //SceneManager.LoadScene("Level01");
    }

    public void Exit()
    {
        playsound.Play();
        Debug.Log("quit");//Debug.log permet d'afficher dans la console differnet output pour justement debugger mon code ici je verifie que lorsque j'appuie sur Exit le bouton me renvois le message quit
        Application.Quit(); 
    }

    public void Settings()
    {
        playsound.Play();
        settingwindow.SetActive(true); //j'affiche le menu des reglages
        exitButton.SetActive(false); //je desactive ou "cache" les boutons du menu principal
        creditButton.SetActive(false);
        playButton.SetActive(false); 
    }

    public void Credits()
    {
        playsound.Play();
        StartCoroutine(loadCredit());
    }

    public void closeSettings()
    {
        playsound.Play();
        settingwindow.SetActive(false);
        exitButton.SetActive(true);
        creditButton.SetActive(true);
        playButton.SetActive(true);
    }

    public IEnumerator loadLevel()
    {
        yield return new WaitForSeconds(0.35f); //attente de 0.35s
        SceneManager.LoadScene("MainScene");
    }
        
    public IEnumerator loadCredit()
    {
        yield return new WaitForSeconds(0.35f);
        SceneManager.LoadScene("Credit");
    }
}   
