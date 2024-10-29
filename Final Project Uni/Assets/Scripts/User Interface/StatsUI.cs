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

    public TMP_Text healthStat;
    public Button hpPlusButton;
    public Button hpMinusButton;

    #endregion

    #region Speed Buttons

    public TMP_Text speedStat;
    public Button speedPlusButton;
    public Button speedMinusButton;

    #endregion

    #region Damage Buttons

    public TMP_Text damageStat;
    public Button damagePlusButton;
    public Button damageMinusButton;

    #endregion

    public PlayerStats playerStats; 

    private void Start()
    {
        //dont use like this, setup drag and drop for menu stuffs, remember those things will got execute by code
        //-> bad performance compare to already loaded by object
        //compact the value intend to add by a whole list or struct sth then execute only once for nbest performance
        //ExampleL InteractableItem use add and remove only by collider -> make sure the event use wont touch others
        
        hpPlusButton.onClick.AddListener(() => playerStats.ModifyPermaStat("HP", true));
        hpMinusButton.onClick.AddListener(() => playerStats.ModifyPermaStat("HP", false));
        speedPlusButton.onClick.AddListener(() => playerStats.ModifyPermaStat("Speed", true));
        speedMinusButton.onClick.AddListener(() => playerStats.ModifyPermaStat("Speed", false));
        damagePlusButton.onClick.AddListener(() => playerStats.ModifyPermaStat("Damage", true));
        damageMinusButton.onClick.AddListener(() => playerStats.ModifyPermaStat("Damage", false));

        confirmModifierPermanentButton.onClick.AddListener(playerStats.ConfirmUpdateStats);
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

    public void UpdateStatsDisplay(int health, float speed, int damage, int gold)
    {
        healthStat.text = $"Health: {health}";
        speedStat.text = $"Speed: {speed:F2}";
        damageStat.text = $"Damage: {damage}";
        goldText.text = $"Power of Player: {gold}";
    }

    private void CloseStatsUI()
    {
        gameObject.SetActive(false);
    }
}