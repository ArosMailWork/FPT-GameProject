using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotGun : MonoBehaviour
{
    public GameObject projectile;

    public void FireGun()
    {
        GameObject.Instantiate(projectile, transform.position, transform.rotation);
    }
}
