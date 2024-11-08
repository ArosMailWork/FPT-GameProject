using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public ExpeditionManager exManager;
    public void Start()
    {
        exManager = GameObject.Find("GenerationManager").GetComponent<ExpeditionManager>();
        gameObject.SetActive(false);
        // if (exManager.CheckBossRoom())
        // {
        //     gameObject.SetActive(false);
        // }
    }
    public void StartPortal()
    {
        exManager.ExitFloor();
    }
    public void ActivatePortal()
    {
        gameObject.SetActive(true);
    }
}
