using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotGun : MonoBehaviour
{
    public GameObject projectile;

    public void FireGun() // animation will call this
    {
        GameObject.Instantiate(projectile, transform.position, transform.rotation);
        //PoolManager.Instance.Spawn(projectile, transform.position, transform.rotation);
    }
}
