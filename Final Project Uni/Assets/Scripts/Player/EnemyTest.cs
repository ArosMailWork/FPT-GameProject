using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public void EnemyHurt()
    {
        Debug.Log("Enemy: ouch");
    }
    
    //TEST
    public void EnemyKnock(DataPack dp)
    {
        Debug.Log("Enemy: KNOCK dp " + dp.hp + " / " + dp.pos + " / " + dp.rot + " / " + dp.mp + " / " + dp.stamina);
    }
}
