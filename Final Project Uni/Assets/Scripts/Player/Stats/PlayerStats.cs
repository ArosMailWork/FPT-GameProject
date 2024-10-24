using System;
using System.Collections;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int playerGold = 1000;

    #region Default

    public int HealthFromPermanent;
    public int knowledgeLevel;

    [FoldoutGroup("Default Stats")]
    public float defaultSpeed = 0.7f,
        defaultRotationSpeed = 25,
        defaultMaxSpeed = 20;

    [FoldoutGroup("Default Stats/Roll")]
    public float defaultRollSpeed = 13,
        defaultRollCD = 0.6f,
        defaultRollTime = 0.25f,
        defaultControlRollDirect = 0.2f;

    [FoldoutGroup("Default Stats/Shooting")]
    public float defaultChargedTime = 2;

    [FoldoutGroup("Default Stats/Guard")]
    public float defaultGuardTime = 2; //temp

    [FoldoutGroup("Default Stats/Stamina")]
    public int defaultMaxStamina = 100,
        defaultRegenRate = 10,
        defaultStaminaRollCost = 20;

    [FoldoutGroup("Default Stats/Arrow")]
    public float defaultRicochetFriction = 0;

    [FoldoutGroup("Default Stats/Arrow")]
    public int defaultDamage = 2;

    [FoldoutGroup("Default Stats/Arrow")]
    public float defaultDamageMultiplier = 1;

    [FoldoutGroup("Default Stats/Physics")]
    [ReadOnly]
    public float defaultDrag,
        defaultMass;

    #endregion

    #region Perma Upgrade
    public StatsUI statsUI;

    [FoldoutGroup("Perman Upgrade")]
    public float permaHP_Percent = 0.02f;

    [FoldoutGroup("Perman Upgrade")]
    public float permaSpeed_Percent = 0.02f;

    [FoldoutGroup("Perman Upgrade")]
    public float permaDamage_Percent = 0.02f;

    [FoldoutGroup("Perman Upgrade/Temps")]
    [ReadOnly]
    public float permanentHP = 1f,
        permanentSpeed = 1f,
        permanentDamage = 1f;
    #endregion

    #region Bonus

    [FoldoutGroup("Bonus Stats")]
    public float bonusSpeed,
        bonusRotationSpeed,
        bonusMaxSpeed;

    [FoldoutGroup("Bonus Stats/Roll")]
    public float bonusRollSpeed,
        bonusRollCD,
        bonusRollTime,
        bonusControlRollDirect;

    [FoldoutGroup("Bonus Stats/Stamina")]
    public int bonusMaxStamina,
        bonusRegenRate,
        bonusStaminaRollCost;

    [FoldoutGroup("Bonus Stats/Arrow Controller")]
    public float bonusChargedTime;

    [FoldoutGroup("Bonus Stats/Arrow")]
    public float bonusRicochetMultiplier;

    [FoldoutGroup("Bonus Stats/Arrow")]
    public int bonusDamage;

    [FoldoutGroup("Bonus Stats/Arrow")]
    public float bonusDamageMultiplier;

    #endregion

    #region Total Value (Calculate)

    //Health
    public int totalMaxHealth => Mathf.CeilToInt(_pc.PlayerHealth.maxHealth * permanentHP);

    //Speed
    public float speed => defaultSpeed * permanentSpeed + bonusSpeed;
    public float rotationSpeed => defaultRotationSpeed + bonusRotationSpeed;
    public float maxSpeed => defaultMaxSpeed + bonusMaxSpeed;

    //Roll
    public float rollSpeed => defaultRollSpeed + bonusRollSpeed;
    public float rollCD => defaultRollCD + bonusRollCD;
    public float rollTime => defaultRollTime + bonusRollTime;
    public float controlRollDirect => defaultControlRollDirect + bonusControlRollDirect;

    //Guard
    public float guardTime => defaultGuardTime; // No bonus, just returns default value

    //Stamina
    public int maxStamina => defaultMaxStamina + bonusMaxStamina;
    public int regenRate => defaultRegenRate + bonusRegenRate;
    public int staminaRollCost => defaultStaminaRollCost - bonusStaminaRollCost;

    //Arrow Controller
    public float ChargedTime => defaultChargedTime - bonusChargedTime;

    //Arrow
    public float staticFriction => defaultRicochetFriction * bonusRicochetMultiplier;

    public int Damage => Mathf.CeilToInt(defaultDamage * permanentDamage) + bonusDamage;

    // nochange * (uptoBigEnough) + uptoBigEnough

    public float DamageMultiplier => defaultDamageMultiplier + bonusDamageMultiplier;

    #endregion

    private PlayerController _pc;
    private StaminaSystem _staminaSystem;
    private ArrowController _arrowController;

    private void Start()
    {
        _pc = PlayerController.Instance;
        _staminaSystem = _pc.staminaSystem;
        _arrowController = _pc._arrowController;
        defaultDrag = _pc.PlayerRB.drag;
        defaultMass = _pc.PlayerRB.mass;

        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return null;
        LoadPermanentStats();
        UpdatePlayerStats();
        UpdateUI();
    }

    public void UpdateUI()
    {
        statsUI.UpdateStatsDisplay(totalMaxHealth, speed, Damage);
    }

    private float upgradeMultiplier = 1;

    public void UpgradeStats(int upgradeType)
    {
        int cost = upgradeType == 2 ? 100 : 200;
        if (playerGold >= cost)
        {
            playerGold -= cost;
            //% sẽ giảm dần theo hàm y = x^0.6
            upgradeMultiplier = Mathf.Pow(upgradeType / 2f, 0.6f);

            int upgradeAmount = upgradeType == 2 ? 1 : 2;
            //tăng số lần đã upgrade lên
            permaHPUpgrades += upgradeAmount;
            permaSpeedUpgrades += upgradeAmount;
            permaDamageUpgrades += upgradeAmount;

            //giá trị tăng % sau khi convert từ số lần đã upgrade, temp thôi
            UpdatePlayerStats();
            UpdateUI();
            statsUI.UpdateGoldDisplay(playerGold);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

    public void UpdatePlayerStats()
    {
        //giá trị tăng % sau khi convert từ số lần đã upgrade(temp thôi)
        permanentHP = 1f + (permaHP_Percent * permaHPUpgrades * upgradeMultiplier);
        permanentSpeed = 1f + (permaSpeed_Percent * permaSpeedUpgrades * upgradeMultiplier);
        permanentDamage = 1f + (permaDamage_Percent * permaDamageUpgrades * upgradeMultiplier);
    }

    public void BuffPlayer(int healthBuff, float speedBuff, int damageBuff)
    {
        _pc.PlayerHealth.maxHealth += healthBuff;
        defaultSpeed += speedBuff;
        defaultDamage += damageBuff;
        UpdatePlayerStats();
        UpdateUI();
    }

    [Button]
    public void UpdateBonusValue()
    {
        //Stamina
        _staminaSystem.MaxStamina = maxStamina;
        _staminaSystem.RegenRate = regenRate;

        //Arrow Controller
        _arrowController.chargedTime = ChargedTime;

        //Arrow
        foreach (var arrow in _arrowController.arrowsList)
        {
            arrow.bonusRicochetMultiplier = bonusRicochetMultiplier;
            arrow.hitbox.BaseDamage = Damage;
            arrow.hitbox.MirageMultiplier = DamageMultiplier;
        }

        //health
        _pc.PlayerHealth.maxHealth = totalMaxHealth;
    }

    // Số lần đã upgrade
    [FoldoutGroup("Permanent Stats")]
    public int permaHPUpgrades = 0;

    [FoldoutGroup("Permanent Stats")]
    public int permaSpeedUpgrades = 0;

    [FoldoutGroup("Permanent Stats")]
    public int permaDamageUpgrades = 0;

    private void LoadPermanentStats()
    {
        PermaStatsData loadedData = PermanCRUD.LoadPermanentStats();
        permaHPUpgrades = loadedData.HPUpgradesData;
        permaSpeedUpgrades = loadedData.SpeedUpgradesData;
        permaDamageUpgrades = loadedData.DamageUpgradesData;
    }

    public void ConfirmStats()
    {
        var data = new PermaStatsData
        {
            HPUpgradesData = permaHPUpgrades,
            SpeedUpgradesData = permaSpeedUpgrades,
            DamageUpgradesData = permaDamageUpgrades,
        };
        PermanCRUD.SavePermanentStats(data);
    }
}

[System.Serializable]
public class PermaStatsData
{
    public int HPUpgradesData;
    public int SpeedUpgradesData;
    public int DamageUpgradesData;
}
