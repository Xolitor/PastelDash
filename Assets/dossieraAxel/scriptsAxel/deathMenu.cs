using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deathMenu : MonoBehaviour
{
    public AudioSource playsound; //ref a cette piste
    public void returnMenu()
    {
        playsound.Play(); //joue la piste audio "playsound"
        StartCoroutine(loadMenu());
    }

    public IEnumerator loadMenu() //chargement de la scene menu si le joueur veut quitter les credits
    {
        yield return new WaitForSeconds(0.05f);
        SceneManager.LoadScene("Menu");
    }
}
