using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotGun : MonoBehaviour
{
    public GameObject projectile;

    public void FireGun()
    {
        GameObject.Instantiate(projectile, transform.position, transform.rotation);
        //PoolManager.Instance.Spawn(projectile, transform.position, transform.rotation);
    }
}
