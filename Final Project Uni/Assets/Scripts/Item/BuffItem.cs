// using PA;
// using UnityEngine;
//
// public class BuffItem : MonoBehaviour
// {
//     public int healthBuff = 100, damageBuff = 10;
//     public float speedBuff = 10f;
//
//     private PlayerStatsManager playerStatsManager;
//
//
//     private void ApplyBuff()
//     {
//         if (playerStatsManager != null)
//         {
//             playerStatsManager.BuffPlayer(healthBuff, speedBuff, damageBuff);
//             Destroy(gameObject); // Remove the buff item after use
//         }
//         else
//         {
//             Debug.LogError("Cannot apply buff: PlayerStatsManager is null!");
//         }
//     }
// }
