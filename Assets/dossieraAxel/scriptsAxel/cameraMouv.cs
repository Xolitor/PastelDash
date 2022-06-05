using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMouv : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(player.position.x, player.position.y , transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.position.x  , player.position.y , transform.position.z);
    }
}
