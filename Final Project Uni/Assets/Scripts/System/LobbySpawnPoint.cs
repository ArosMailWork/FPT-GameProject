using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySpawnPoint : MonoBehaviour
{
    public GameObject p;
    // Start is called before the first frame update
    void Start()
    {
        p = GameObject.Find("Player");
        p.transform.position = transform.position;
    }


}
