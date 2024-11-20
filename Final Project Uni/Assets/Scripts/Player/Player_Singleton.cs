using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Singleton : MonoBehaviour
{
    public static Player_Singleton Instance;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}
