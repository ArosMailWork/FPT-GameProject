using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Slider healthSlider;
    public Slider staminaSlider;
    public TMP_Text healthText;
    public TMP_Text staminaText;

    public Button ShootButton;
    public Button RollButton;
    public Button RecallButton;
    public Button MeleeButton;

    private PlayerController playerController;
    public static PlayerUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        playerController = PlayerController.Instance;
        //playerController.OnHealthChanged += UpdateHealthUI;
        playerController.staminaSystem.OnValueChange += UpdateStaminaUI;
        // playerController.OnPlayerDeath += DisableUI;
        
        ShootButton.onClick.AddListener(playerController.Shoot);
        RollButton.onClick.AddListener(playerController.Roll);
        RecallButton.onClick.AddListener(ArrowController.Instance.Recall);
        MeleeButton.onClick.AddListener(playerController.MeleeAttack);

    }
    
    
    private void UpdateHealthUI(float health)
    {
        healthSlider.value = health;
        healthText.text = health.ToString();
    }
    
    private void UpdateStaminaUI(object stamina)
    {
        //maybe can use object stamina 
        staminaSlider.value = playerController.staminaSystem.Value;
        staminaText.text = playerController.staminaSystem.Value.ToString();
    }

    public void HidePlayerUI()
    {
        transform.gameObject.SetActive(false);
    }
    
    public void ShowPlayerUI()
    {
        transform.gameObject.SetActive(true);
    }
}
