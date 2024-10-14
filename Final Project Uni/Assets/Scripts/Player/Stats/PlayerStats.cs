using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Default
    
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
    
    #endregion
    
    #region Bonus

    [FoldoutGroup("Bonus Stats")]
    public float bonusSpeed, bonusRotationSpeed, bonusMaxSpeed;
    [FoldoutGroup("Bonus Stats/Roll")]
    public float bonusRollSpeed, bonusRollCD, bonusRollTime, bonusControlRollDirect;
    [FoldoutGroup("Bonus Stats/Stamina")]
    public int bonusMaxStamina, bonusRegenRate, bonusStaminaRollCost;

    [FoldoutGroup("Bonus Stats/Arrow Controller")]
    public float bonusChargedTime;
    
    
    #endregion

    #region Total Value

    // Speed
    public float speed => defaultSpeed + bonusSpeed;
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
    
    //Physics
    [ReadOnly] public float defautDrag, defaultMass;
    
    #endregion

    private PlayerController _pc;
    private StaminaSystem _staminaSystem;
    private ArrowController _arrowController;

    private void Start()
    {
        _pc = PlayerController.Instance;
        _staminaSystem = _pc.staminaSystem;
        _arrowController = _pc._arrowController;
        defautDrag = _pc.PlayerRB.drag;
        defaultMass = _pc.PlayerRB.mass;
    }

    public void UpdateBonusValue()
    {
        //Stamina
        _staminaSystem.MaxStamina = maxStamina;
        _staminaSystem.RegenRate = regenRate;
        
        //Arrow Controller
        _arrowController.chargedTime = ChargedTime;
    }
}