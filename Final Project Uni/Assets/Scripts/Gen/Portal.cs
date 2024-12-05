using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [HideInInspector] public ExpeditionManager exManager;
    public void Start()
    {
        exManager = ExpeditionManager.Instance;
        //exManager = GameObject.Find("GenerationManager").GetComponent<ExpeditionManager>();
    }

    [Button]
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
        GameManager.Instance.changeColorTransition.Invoke(Color.gray);
        GameManager.Instance.StartExpedition();
    }
}
