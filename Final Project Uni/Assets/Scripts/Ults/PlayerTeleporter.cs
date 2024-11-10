using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    void Start()
    {
        PlayerController.Instance.PlayerRB.transform.position = transform.position;
    }
}
