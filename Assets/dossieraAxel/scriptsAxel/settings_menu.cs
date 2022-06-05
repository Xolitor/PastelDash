using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;

public class settings_menu : MonoBehaviour
{
    public Dropdown resolutionDropdown; //ref au menu deroulant pour les resolutions
    public AudioMixer audioM; //ref a l'audioMixer (pour les sons)
    Resolution[] resolutions;

    public void Start()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        resolutionDropdown.ClearOptions(); //on efface les elements ecrits dans le menu deroulant de base (Option A, Option B, Option C)

        List<string> options = new List<string>(); //une liste de chaine de charactere qui va contenir les options
        int currentIndexRes = 0; //l'index qui va nous permettre d'attribuer une resolution initiale

        for (int i =0; i < resolutions.Length; i++)
        {

            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option); //on parcours le  tableau de resolutions et on ajoute chacune d'elle a notre liste d'options

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height) //lorsqu'on parcours les resolutions si une respecte parfaitement les dimensions alors on l'a choisis
            {
                currentIndexRes = i;
                resolutionDropdown.value = currentIndexRes; //dans notre menu deroulant on se positionne au niveau de cette resolution pour indiquer a l'utilisateur laquelle c'est
            }
        }

        resolutionDropdown.AddOptions(options); //Une fois le tableu parcourus on ajoute la liste complete dans le menu deroulant
        resolutionDropdown.value = currentIndexRes; 
        resolutionDropdown.RefreshShownValue();
        Screen.fullScreen = true;
    }

    public void SetAudio (float vol)
    {
        audioM.SetFloat("masterSound", vol); //la valeur "vol" attribue a l'audioMixer qui s'appelle "masterSound"
        Debug.Log(vol);
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void Setresolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex]; //choisi la resolution souhaite par l'utilisateur
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen); //applique la resolution et verifie si c'est fentre ou pas
    }
}
