using PA;
using UnityEngine;

public enum BuffType
{
    Health, Speed, Damage
}

public class BuffItem : MonoBehaviour
{
    public BuffType type;
    public int amount;
    
     public void ApplyBuff()
     {
         PlayerController.Instance._stats.BuffPlayer(type, amount);
     }
 }
