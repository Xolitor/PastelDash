using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class credit_menu : MonoBehaviour
{
    public AudioSource playsound; //ref a cette piste
    public void returnMenu()
    {
        playsound.Play(); //joue la piste audio "playsound"
        StartCoroutine(loadMenu());
    }

    public IEnumerator loadMenu() //chargement de la scene menu si le joueur veut quitter les credits
    {
        SceneManager.LoadScene("Menu");
        yield return null;
    }
}
