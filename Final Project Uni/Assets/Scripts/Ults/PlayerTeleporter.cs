using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    void OnEnable()
    {
        PlayerController.Instance.PlayerRB.transform.position = transform.position;
        
        PlayerController.Instance.ResetStat();
        PlayerController.Instance.Revive(1, 0);
    }
}
