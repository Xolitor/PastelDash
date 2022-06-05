using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathscrennColor : MonoBehaviour
{
    public GameObject quad;
    Renderer quadRenderer;

    public float lerpDuration = 24.0f;
    public float smoothnessGlobal = 0.005f;
    int i = 0;


    public List<Color32> listColorSet = new List<Color32>()
    {
        
            new Color32(38, 70 , 83 , 255),
            new Color32(42, 157, 143, 255),
            new Color32(233, 196, 106, 255),
            new Color32(244, 162, 97, 255),
            new Color32(231, 111, 81, 255)
    };


    private void Start()
    {
        quadRenderer = quad.GetComponent<Renderer>();
        StartCoroutine(SkinColor());
    }

    private IEnumerator SkinColor() //change progressivement la couleur du skin du player 
    {
        while (true)
        {
            if (i==0)
            {
                Color32 prevBG = quadRenderer.material.color;
                Color32 nextColorBG = listColorSet[Random.Range(0, 4)];
                //Prends une couleur aléatoire parmis le set de couleur courant (set défini par le niveau courant)


                //Debug.LogWarning("New skin : " + nextColor);
                float progress = 0;
                float increment = smoothnessGlobal / lerpDuration; //The amount of change to apply.

                while (!quadRenderer.material.color.Equals(nextColorBG))
                {
                    //currentTime += Time.deltaTime;
                    quadRenderer.material.color = Color.Lerp(prevBG, nextColorBG, progress);
                    progress += increment;
                    yield return null;
                }
                yield return null;
            }
        }
    }
}

