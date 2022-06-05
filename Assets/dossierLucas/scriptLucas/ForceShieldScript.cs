using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceShieldScript : MonoBehaviour // Gestion de l'affichage du ForceShield
{
    public GameController gameController;
    Text textFS;
    // Start is called before the first frame update
    void Start()
    {
        textFS = GetComponent<Text>();
        textFS.text = "ForceShield : 100" + gameController.currentForceShield.ToString(); //Texte par défaut
    }

    // Update is called once per frame
    void Update()
    {
        
        textFS.text = "ForceShield : " + ((int)gameController.currentForceShield).ToString();
        //uptade le contenu du texte
        //Debug.Log("textFS.text = " + textFS.text);
        //textFS.enabled = (gameController.currentForceShield != 100);
        //Affiche le texte ssi power != 100
    }

}
