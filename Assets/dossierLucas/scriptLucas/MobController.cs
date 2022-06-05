using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MobController : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    private SpriteRenderer rend;
    public GameController gameController;
    public ScoreController scoreController;
    PlayerControl playercontrol;
    //public GameObject maincamera;

    void Start()
    {
        player = GameObject.Find("Player");
        rend = player.GetComponent<SpriteRenderer>();
        playercontrol = player.GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        // Alias
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;
        float mobX = transform.position.x;
        float mobY = transform.position.y;

        // Alias
        bool cond1 = (playerX + 0.5 * player.transform.localScale.x) < mobX - 0.5 * transform.localScale.x;
        bool cond2 = (playerY + 0.5 * player.transform.localScale.y) < mobY - 0.5 * transform.localScale.y;
        bool cond3 = (playerX - 0.5 * player.transform.localScale.x) > mobX + 0.5 * transform.localScale.x;
        bool cond4 = (playerY - 0.5 * player.transform.localScale.y) > mobY + 0.5 * transform.localScale.y;

        bool contactHitBox = !(cond1 || cond2 || cond3 || cond4);
        // si une des conditions n'est pas respecté, alors il n'y a pas de superposition entre le joueur est le mob => pas de contact

        if (contactHitBox)
        {
            if ( gameController.isForceShieldOn) //si FS actif
            {
                //Debug.Log("Kill Mob");
                gameController.currentGameEvent.Add(GameController.GameEvent.EnnemyKill); // On ajoute l'event à la liste
                scoreController.bonusScore += 5; // Ajout d'un bonus score
                Destroy(gameObject); // Suppression du mob
            }

            else // sinon, changement d'état du jeu vers GameOver pour l'instant
            {
                rend.enabled = false;
                //maincamera.GetComponent<cameraMouv>().enabled = false;
                StartCoroutine(deathanim());
                //playercontrol.totalMovementTime = 0f;
                gameController.currentStateOfGame = GameController.StateOfGame.GameOver;
                //Debug.Log("Player died");
            }
        }
    }

    IEnumerator deathanim()
    {
        yield return new WaitForSeconds(0.35f);
        SceneManager.LoadScene("DeathScreen");
    }
}
