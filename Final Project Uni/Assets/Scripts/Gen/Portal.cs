using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject genManager;
    public void Start()
    {
        genManager = GameObject.Find("GenerationManager");
    }
    public void StartPortal()
    {
        genManager.GetComponent<ExpeditionManager>().ExitFloor();
    }
}
