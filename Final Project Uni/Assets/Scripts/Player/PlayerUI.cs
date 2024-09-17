using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerUI : MonoBehaviour
{
    public Slider healthSlider;
    public Slider staminaSlider;
    public TMP_Text healthText;
    public TMP_Text staminaText;

    public Button ShootButton;
    public Button RollButton;

    public float meleeHoldTime = 2;
    public Button MeleeButton;

    private bool isMeleeButtonDown = false;
    private float meleeButtonHoldTime = 0;

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
        playerController.staminaSystem.OnValueChange += UpdateStaminaUI;

        ShootButton.onClick.AddListener(ArrowController.Instance.Shoot);
        RollButton.onClick.AddListener(playerController.Roll);

        AddEventTrigger(MeleeButton.gameObject, EventTriggerType.PointerDown, OnMeleeButtonDown);
        AddEventTrigger(MeleeButton.gameObject, EventTriggerType.PointerUp, OnMeleeButtonUp);
    }

    private void Update()
    {
        if (isMeleeButtonDown)
        {
            meleeButtonHoldTime += Time.deltaTime;
            if (meleeButtonHoldTime >= meleeHoldTime)
            {
                ArrowController.Instance.Recall();
                isMeleeButtonDown = false;
            }
        }
    }

    private void OnMeleeButtonDown(BaseEventData data)
    {
        isMeleeButtonDown = true;
        meleeButtonHoldTime = 0;
    }

    private void OnMeleeButtonUp(BaseEventData data)
    {
        if (meleeButtonHoldTime < meleeHoldTime)
        {
            playerController.MeleeAttack();
        }
        isMeleeButtonDown = false;
    }

    private void AddEventTrigger(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>() ?? obj.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    private void UpdateHealthUI(float health)
    {
        healthSlider.value = health;
        healthText.text = health.ToString();
    }

    private void UpdateStaminaUI(object stamina)
    {
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