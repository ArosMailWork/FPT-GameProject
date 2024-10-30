using System;
using System.Collections;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int playerGold = 0;

    #region Default
    [SerializeField]
    private int defaultMaxHealth = 10;
    public int knowledgeLevel;

    [FoldoutGroup("Default Stats")]
    public float defaultSpeed = 0.7f, defaultRotationSpeed = 25, defaultMaxSpeed = 20;

    [FoldoutGroup("Default Stats/Roll")]
    public float defaultRollSpeed = 13, defaultRollCD = 0.6f, defaultRollTime = 0.25f, defaultControlRollDirect = 0.2f;

    [FoldoutGroup("Default Stats/Shooting")]
    public float defaultChargedTime = 2;

    [FoldoutGroup("Default Stats/Guard")]
    public float defaultGuardTime = 2; //temp

    [FoldoutGroup("Default Stats/Stamina")]
    public int defaultMaxStamina = 100, defaultRegenRate = 10, defaultStaminaRollCost = 20;

    [FoldoutGroup("Default Stats/Arrow")]
    public float defaultRicochetFriction = 0;

    [FoldoutGroup("Default Stats/Arrow")]
    public int defaultDamage = 2;

    [FoldoutGroup("Default Stats/Arrow")]
    public const float defaultDamageMultiplier = 1;

    [FoldoutGroup("Default Stats/Physics")]
    [ReadOnly]
    public float defaultDrag,
        defaultMass;

    #endregion

    #region Perma Upgrade
    // Số lần đã upgrade
    [FoldoutGroup("Permanent Stats")]
    public int permaHP_UpAmount = 0, permaSpeed_UpAmount = 0, permaDamage_UpAmount = 0;
    
    [FoldoutGroup("Permanent Stats/Debug Calculate")]
    [ReadOnly] public float PermaHP_Percent = 1f, PermaSpeed_Percent = 1f, PermaDamage_Percent = 1f;
    #endregion

    #region Bonus

    [FoldoutGroup("Bonus Stats")]
    public float bonusSpeed, bonusHealth, bonusRotationSpeed, bonusMaxSpeed;

    [FoldoutGroup("Bonus Stats/Roll")]
    public float bonusRollCD, bonusRollTime, bonusControlRollDirect;

    [FoldoutGroup("Bonus Stats/Stamina")]
    public int bonusMaxStamina, bonusRegenRate, bonusStaminaRollCost;

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
    public int totalMaxHealth => Mathf.CeilToInt((defaultMaxHealth * PermaHP_Percent) + bonusHealth);

    //Speed
    public float speed => (defaultSpeed * PermaSpeed_Percent) + bonusSpeed;
    public float rotationSpeed => defaultRotationSpeed + bonusRotationSpeed;
    public float maxSpeed => defaultMaxSpeed + bonusMaxSpeed;

    //Roll
    public float rollSpeed => defaultRollSpeed + (PermaSpeed_Percent * defaultSpeed);
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

    public int Damage => Mathf.CeilToInt(((defaultDamage * PermaDamage_Percent) + bonusDamage) * DamageMultiplier);
    public float DamageMultiplier => defaultDamageMultiplier + bonusDamageMultiplier;

    #endregion

    [SerializeField, ReadOnly] private PlayerController _pc;
    private StaminaSystem _staminaSystem;
    private ArrowController _arrowController;
    
    private void Start()
    {
        _pc = PlayerController.Instance;
        _staminaSystem = _pc.staminaSystem;
        _arrowController = _pc._arrowController;
        defaultDrag = _pc.PlayerRB.drag;
        defaultMass = _pc.PlayerRB.mass;
        LoadSave();
        UpdateStats();
    }
    
    //in-game buff
    public void BuffPlayer(BuffType type, float amount = 0)
    {
        switch (type)
        {
            case BuffType.Health:
                bonusHealth += amount;
                
                //Set HP
                _pc.PlayerHealth.maxHealth = totalMaxHealth;
                _pc.PlayerHealth.currentHealth = totalMaxHealth;
                break;
            case BuffType.Damage:
                bonusDamage += Mathf.CeilToInt(amount);
                //Set Damage Through a command setup at Arrow Controller, Duck will make it later
                
                break;
            case BuffType.Speed:
                bonusSpeed += amount;
                break;
        }

        UpdateStats();
    }

    [Button]
    public void UpdateStats()
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

    public void LoadSave()
    {
        PermaStatsData loadedData = PermanCRUD.LoadPermanentStats();
        playerGold = loadedData.Gold;
        permaHP_UpAmount = loadedData.HPUpgradesData;
        permaSpeed_UpAmount = loadedData.SpeedUpgradesData;
        permaDamage_UpAmount = loadedData.DamageUpgradesData;

        UpdatePermaPercent();
    }

    public void UpdatePermaPercent()
    {
        PermaHP_Percent = 1f + (CalculateMultiplier(permaHP_UpAmount));
        PermaSpeed_Percent = 1f + (CalculateMultiplier(permaSpeed_UpAmount));
        PermaDamage_Percent = 1f + (CalculateMultiplier(permaDamage_UpAmount));
    }
    
    public float CalculateMultiplier(int amount)
    {
        return (Mathf.Pow(amount, 0.6f)) * 0.01f;
    }
}

