using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BossSkill : MonoBehaviour
{
    public int phase = 0;
    
    [FoldoutGroup("Setup")]
    public List<BotGun> gun;
    
    public void Attack(Rigidbody rb)
    {
        
    }
}
