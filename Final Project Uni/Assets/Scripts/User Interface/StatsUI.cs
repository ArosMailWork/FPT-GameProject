using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PA
{
    public class StatsUI : MonoBehaviour
    {
        public TMP_Text healthStat, speedStat, damageStat, goldText;

        public Button exitStatsButton, confirmModifierPermanentButton, upgradeButtonx2, upgradeButtonx5;

        public PlayerStatsManager statsManager;

        private void Start()
        {
            upgradeButtonx2.onClick.AddListener(() => statsManager.UpgradeStats(0.02f));
            upgradeButtonx5.onClick.AddListener(() => statsManager.UpgradeStats(0.04f));
            confirmModifierPermanentButton.onClick.AddListener(statsManager.ConfirmStats);
            exitStatsButton.onClick.AddListener(CloseStatsUI);
            UpdateGoldDisplay(statsManager.playerGold);
        }

        public void UpdateStatsDisplay(int health, float speed, int damage)
        {
            healthStat.text = $"Health: {health}";
            speedStat.text = $"Speed: {speed:F2}";
            damageStat.text = $"Damage: {damage}";
        }

        public void UpdateGoldDisplay(int gold)
        {
            goldText.text = $"Gold: {gold}";
        }

        private void CloseStatsUI()
        {
            gameObject.SetActive(false);
        }
    }
}