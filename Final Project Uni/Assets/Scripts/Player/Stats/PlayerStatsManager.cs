using System;
using System.Collections;
using UnityEngine;

namespace PA
{
    public class PlayerStatsManager : MonoBehaviour
    {
        public PlayerStats playerStats;
        public StatsUI statsUI;
        public float percentHp;
        public float percentSpeed;
        public float percentDamage;

        public int playerGold = 1000; // Giả lập lượng vàng

        private void Start()
        {
            //delay 1 frame to get player stats
            StartCoroutine(DelayedStart());
        }

        private IEnumerator DelayedStart()
        {
            yield return null;
            playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
            PermanentStats.LoadStats();
            UpdatePlayerStats();
            UpdateUI();
        }

        //update to display for player base on data from total value when play
        //value display will be Round to Int mean different from actual value
        public void UpdatePlayerStats()
        {
            //subtract because after upgrade, the value is more than 1, end we only need increase percentage amount
            playerStats.bonusSpeed = playerStats.defaultSpeed * (PermanentStats.Speed - 1f);
            playerStats.bonusDamage = Mathf.CeilToInt(
                playerStats.defaultDamage * (PermanentStats.Damage - 1f)
            );
            print("bonusDamage: " + playerStats.bonusDamage);
            playerStats.HealthFromPermanent = Mathf.CeilToInt(
                playerStats.playerHealth * (PermanentStats.HP - 1f)
            ); // 10 * (1.02-1)
            playerStats.UpdateBonusValue();
        }

        public void UpdateUI()
        {
            // use data from total value
            statsUI.UpdateStatsDisplay(
                playerStats.playerHealth + playerStats.HealthFromPermanent,
                playerStats.speed,
                playerStats.Damage
            );
        }

        //update actual value
        public void UpgradeStats(int upgradeType)
        {
            int cost = upgradeType == 2 ? 100 : 200; // 100 gold for 2%, 200 gold for 4%
            if (playerGold >= cost)
            {
                playerGold -= cost;
                PermanentStats.UpdateStat("HP", PermanentStats.HP + percentHp);
                PermanentStats.UpdateStat("Speed", PermanentStats.Speed + percentSpeed);
                PermanentStats.UpdateStat("Damage", PermanentStats.Damage + percentDamage);
                UpdatePlayerStats();
                UpdateUI();
                statsUI.UpdateGoldDisplay(playerGold);
            }
            else
            {
                Debug.Log("Not enough gold!");
            }
        }

        public void ConfirmStats()
        {
            PermanentStats.SaveStats();
            Debug.Log(Application.dataPath + "/player_stats.json");
            Debug.Log("Stats saved permanently!");
        }
    }
}
