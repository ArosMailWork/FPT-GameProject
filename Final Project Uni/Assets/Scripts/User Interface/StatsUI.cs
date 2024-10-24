using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    public TMP_Text healthStat,
        speedStat,
        damageStat,
        goldText;

    public Button exitStatsButton,
        confirmModifierPermanentButton,
        upgradeButtonx2,
        upgradeButtonx5;

    public PlayerStats playerStats;

    private void Start()
    {
        upgradeButtonx2.onClick.AddListener(() => playerStats.UpgradeStats(2));
        upgradeButtonx5.onClick.AddListener(() => playerStats.UpgradeStats(4));
        confirmModifierPermanentButton.onClick.AddListener(playerStats.ConfirmStats);
        exitStatsButton.onClick.AddListener(CloseStatsUI);
        UpdateGoldDisplay(playerStats.playerGold);
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