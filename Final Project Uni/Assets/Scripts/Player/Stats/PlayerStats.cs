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

    //Speed
    public float speed => (defaultSpeed * permanentSpeed) + bonusSpeed;
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

    public int Damage => Mathf.CeilToInt((defaultDamage * permanentDamage) + bonusDamage);

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

    public void UpdatePlayerStats()
    {
        bonusSpeed = defaultSpeed * (permanentSpeed - 1f);
        bonusDamage = Mathf.CeilToInt(defaultDamage * (permanentDamage - 1f));
        HealthFromPermanent = Mathf.CeilToInt(_pc.PlayerHealth.maxHealth * (permanentHP - 1f));
        UpdateBonusValue();
    }

    public void UpdateUI()
    {
        statsUI.UpdateStatsDisplay(_pc.PlayerHealth.maxHealth, speed, Damage);
    }

    public void UpgradeStats(int upgradeType)
    {
        int cost = upgradeType == 2 ? 100 : 200;
        if (playerGold >= cost)
        {
            playerGold -= cost;
            //% sẽ giảm dần theo hàm y = x^0.6
            float upgradeMultiplier = Mathf.Pow(upgradeType / 2f, 0.6f);
            permanentHP += permaHP_Percent * upgradeMultiplier;
            permanentSpeed += permaSpeed_Percent * upgradeMultiplier;
            permanentDamage += permaDamage_Percent * upgradeMultiplier;
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
        var data = new PermanentStatsData
        {
            HP = permanentHP,
            Speed = permanentSpeed,
            Damage = permanentDamage,
        };
        PermanCRUD.SavePermanentStats(data);
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
        _pc.PlayerHealth.maxHealth += HealthFromPermanent;
    }

    // Add permanent stat fields
    [FoldoutGroup("Debug/Permanent Stats")]
    private float permanentHP = 1f;

    [FoldoutGroup("Debug/Permanent Stats")]
    private float permanentSpeed = 1f;

    [FoldoutGroup("Debug/Permanent Stats")]
    private float permanentDamage = 1f;

    private void LoadPermanentStats()
    {
        PermanentStatsData loadedData = PermanCRUD.LoadPermanentStats();
        permanentHP = loadedData.HP;
        permanentSpeed = loadedData.Speed;
        permanentDamage = loadedData.Damage;
    }

    private void SavePermanentStats()
    {
        string savePath = Path.Combine(Application.dataPath, "player_stats.json");
        var data = new PermanentStatsData
        {
            HP = permanentHP,
            Speed = permanentSpeed,
            Damage = permanentDamage,
        };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }
}
[System.Serializable]
public class PermanentStatsData
{
    public float HP;
    public float Speed;
    public float Damage;
}