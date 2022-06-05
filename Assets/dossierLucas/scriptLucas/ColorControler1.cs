using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorControler1 : MonoBehaviour
{   
    public float deltaErrorTime = 80.0f;

    public float lerpDuration = 24.0f;
    public float smoothnessGlobal = 0.005f;
    public float smoothnessBoss = 0.05f;

    public Material terrainMaterial;

    Renderer rend;
    public GameController gameController;
    public GameObject ground;

    public GameObject quad; 
    Renderer quadRenderer;

    public GameObject square;
    SpriteRenderer spriteRendererSquare;

    public List<List<Color32>> listColorSet = new List<List<Color32>>()
    {
        new List<Color32>()
        {
            new Color32(38, 70 , 83 , 255),
            new Color32(42, 157, 143, 255),
            new Color32(233, 196, 106, 255),
            new Color32(244, 162, 97, 255),
            new Color32(231, 111, 81, 255)
        },
        new List<Color32>()
        {
            new Color32(39, 199, 212, 255),
            new Color32(255, 255, 255, 255),
            new Color32(253, 240, 231, 255),
            new Color32(254, 144, 99, 255),
            new Color32(234, 88, 99, 255)
        },
        new List<Color32>()
        {
            new Color32(0, 0, 0, 255),
            new Color32(254, 39, 126, 255),
            new Color32(252, 254, 25, 255),
            new Color32(71, 234, 208, 255),
            new Color32(255, 255, 255, 255)
        }
    };


    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        quadRenderer = quad.GetComponent<Renderer>();
        spriteRendererSquare = square.GetComponent<SpriteRenderer>();

        string defaulft = nameof(SwitchRandomColor);
        StartCoroutine(defaulft);
        StartCoroutine(BackGroundColor());
        StartCoroutine(MainSwitch(defaulft));
    }

    // Update is called once per frame
    void Update()
    {
        //ground.GetComponent<Renderer>().material.color = this.rend.material.color; //couleur sol = couleur plafond
        if (gameController.currentGameEvent.Contains(GameController.GameEvent.LevelUp))
        {
            //Debug.LogWarning("Switch Skin");
            StartCoroutine(SkinColor());
        }

    }
    private IEnumerator SkinColor() //change progressivement la couleur du skin du player 
    {
        if (gameController.isGameRunning)
        {
            Color32 prev = spriteRendererSquare.color;
            Color32 nextColor = listColorSet[gameController.currentLevel % (listColorSet.Count)]
                [Random.Range(0, listColorSet[gameController.currentLevel % (listColorSet.Count)].Count)];
            //Prends une couleur aléatoire parmis le set de couleur courant (set défini par le niveau courant)

            
            //Debug.LogWarning("New skin : " + nextColor);
            float progress = 0;
            float increment = smoothnessGlobal / lerpDuration; //The amount of change to apply.

            while (!spriteRendererSquare.color.Equals(nextColor)) //tant que la stransition n'est pas terminé
            {
                progress += increment;
                spriteRendererSquare.color = Color.Lerp(prev, nextColor, progress); //changement progressif de couleur
                yield return null;
            }
            yield return null;
        }
    }
    public IEnumerator MainSwitch(string firstCoroutine) // méthode de changement de couleur du terrain selon l'état du jeu
    {
        string currentCoroutineCelling = firstCoroutine;

        while (true)
        {
            if (gameController.isGameRunning)
            {
                switch (gameController.currentStateOfGame)
                {
                    case GameController.StateOfGame.Normal:
                        if (currentCoroutineCelling != "switchRandomColor")
                        {
                            //Debug.Log("Coroutine Stopped");
                            //Debug.Log("current co = " + currentCoroutineCelling);
                            StopCoroutine(currentCoroutineCelling);
                            currentCoroutineCelling = "switchRandomColor";
                        }
                        StartCoroutine(SwitchRandomColor());
                        yield return new WaitForSeconds(lerpDuration);
                        break;

                    case GameController.StateOfGame.Boss:
                        if (currentCoroutineCelling != "switchBossColor")
                        {
                            StopCoroutine(currentCoroutineCelling);
                            currentCoroutineCelling = "switchBossColor";
                        }
                        StartCoroutine(SwitchBossColor());
                        yield return new WaitForSeconds(lerpDuration);
                        break;


                    default:
                        if (currentCoroutineCelling != "switchRandomColor")
                        {
                            //Debug.Log("Coroutine Stopped");
                            //Debug.Log("current co = " + currentCoroutineCelling);
                            StopCoroutine(currentCoroutineCelling);
                            currentCoroutineCelling = "switchRandomColor";
                        }
                        StartCoroutine(SwitchRandomColor());
                        yield return new WaitForSeconds(lerpDuration);
                        break;
                }
            }
            yield return null;
        }
    }
    public IEnumerator SwitchRandomColor() // Méthode de changement progressif de la couleur du terrain 
    {
        Color32 prev = terrainMaterial.color;
        Color32 nextColor = new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 1);
        //Choisi une couleur aléatoire

        float progress = 0;
        float increment = smoothnessGlobal / lerpDuration;  //la quantite de changement a appliquer 

        while (!terrainMaterial.color.Equals(nextColor)) // tant que la couleur suivante n'est pas atteinte
        {
            progress += increment;
            terrainMaterial.color = Color.Lerp(prev, nextColor, progress); // Changement progressif de la couleur vers la couleurs objectif
            yield return null;
        }
        yield return null;
    }
    public IEnumerator BackGroundColor() // Changement progressif de la couleur du background
    {

        while (true)
        {
            if(gameController.isGameRunning)
            {
                Color32 prevBG = quadRenderer.material.color;
                Color32 nextColorBG = listColorSet[gameController.currentLevel % (listColorSet.Count)]
                    [Random.Range(0, listColorSet[gameController.currentLevel % (listColorSet.Count)].Count)];

                while (nextColorBG.Equals(spriteRendererSquare.color))
                {
                    nextColorBG = listColorSet[gameController.currentLevel % (listColorSet.Count)]
                    [Random.Range(0, listColorSet[gameController.currentLevel % (listColorSet.Count)].Count)];
                }
                //Prends une couleur aléatoire parmis le set de couleur courant différente de celle du player (set défini par le niveau courant)

                //Debug.Log("Set n° " + gameController.currentLevel % (listColorSet.Count));
                //float currentTime = 0;


                float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
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
            yield return null;
        }
    }
    public IEnumerator SwitchBossColor() // Méthode de changement de couleur lorsque état du jeu = Boss
    {
        Color32 prev = rend.material.color;
        Color32 nextColor = Color.red;

        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothnessBoss / lerpDuration; //The amount of change to apply.

        while (!rend.material.color.Equals(nextColor))
        {
            progress += increment;
            rend.material.color = Color.Lerp(prev, nextColor, progress);
            yield return null;
        }
        yield return null;
    }
}

