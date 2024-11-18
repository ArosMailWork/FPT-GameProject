using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [HideInInspector] public ExpeditionManager exManager;
    public void Start()
    {
        exManager = ExpeditionManager.Instance;
    }
    public void StartPortal()
    {
        exManager.ExitFloor();
    }
    
    public void ActivatePortal()
    {
        gameObject.SetActive(true);
    }

    public void HubPortal()
    {
        GameManager.Instance.changeColorTransition.Invoke(Color.white);
        GameManager.Instance.StartExpedition();
    }
}
