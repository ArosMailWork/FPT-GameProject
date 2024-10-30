using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    public TMP_Text goldText;
    public Button exitStatsButton;
    public Button confirmModifierPermanentButton;

    #region HP Buttons
    [Header("HP")]
    public TMP_Text healthStat;
    public Button hpPlusButton;
    public Button hpMinusButton;

    #endregion

    #region Speed Buttons
    [Header("Speed")]
    public TMP_Text speedStat;
    public Button speedPlusButton;
    public Button speedMinusButton;

    #endregion

    #region Damage Buttons
    [Header("Damage")]
    public TMP_Text damageStat;
    public Button damagePlusButton;
    public Button damageMinusButton;

    #endregion

    public PlayerStats playerStats;

    private void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

        hpPlusButton.onClick.AddListener(() => playerStats.ModifyStat("HP", true));
        hpMinusButton.onClick.AddListener(() => playerStats.ModifyStat("HP", false));
        speedPlusButton.onClick.AddListener(() => playerStats.ModifyStat("Speed", true));
        speedMinusButton.onClick.AddListener(() => playerStats.ModifyStat("Speed", false));
        damagePlusButton.onClick.AddListener(() => playerStats.ModifyStat("Damage", true));
        damageMinusButton.onClick.AddListener(() => playerStats.ModifyStat("Damage", false));

        confirmModifierPermanentButton.onClick.AddListener(playerStats.ConfirmStats);
        exitStatsButton.onClick.AddListener(CloseStatsUI);
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return null;
        UpdateStatsDisplay(
            playerStats.totalMaxHealth,
            playerStats.speed,
            playerStats.Damage,
            playerStats.playerGold
        );
    }

    private void OnEnable()
    {
        StartCoroutine(DelayedStart());
        Debug.Log("StatsUI OnEnable");
    }

    public void UpdateStatsDisplay(int health, float speed, int damage, int gold)
    {
        healthStat.text = $"Health: {health}";
        speedStat.text = $"Speed: {speed:F2}";
        damageStat.text = $"Damage: {damage}";
        goldText.text = $"Power of Player: {gold}";
    }

    private void CloseStatsUI()
    {
        //cancel changes
        playerStats.CancelChanges();
        //turn off the UI
        gameObject.SetActive(false);
    }
}
