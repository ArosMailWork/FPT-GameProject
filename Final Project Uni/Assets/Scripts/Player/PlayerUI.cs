using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerUI : MonoBehaviour
{
    public Slider healthSlider;
    public Slider staminaSlider;
    public Slider forceSlider;
    public TMP_Text healthText;
    public TMP_Text staminaText;
    public GameObject UltimateCanvas;

    public Button ShootButton;
    public Button RecallButton;
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

        RollButton.onClick.AddListener(playerController.Roll);
        
        AddEventTrigger(RecallButton.gameObject, EventTriggerType.PointerDown, ArrowController.Instance.Recall);
        AddEventTrigger(RecallButton.gameObject, EventTriggerType.PointerUp, ArrowController.Instance.OffRecall);

        AddEventTrigger(ShootButton.gameObject, EventTriggerType.PointerDown, ArrowController.Instance.ChargeShoot);
        //full charged is automatic shoot, dont need to pointer up
        AddEventTrigger(ShootButton.gameObject, EventTriggerType.PointerUp, ArrowController.Instance.Shoot);

        AddEventTrigger(MeleeButton.gameObject, EventTriggerType.PointerDown, OnMeleeButtonDown);
        AddEventTrigger(MeleeButton.gameObject, EventTriggerType.PointerUp, OnMeleeButtonUp);


        forceSlider.maxValue = ArrowController.Instance.chargedTime;
    }

    private void Update()
    {
        if (isMeleeButtonDown)
        {
            meleeButtonHoldTime += Time.deltaTime;
            if (meleeButtonHoldTime >= meleeHoldTime)
            {
                // hide arrow if it over time
                ArrowController.Instance.HideArrow();
                isMeleeButtonDown = false;
            }
        }

        UpdateForceUI(ArrowController.Instance.currentChargedTime);

        MeleeButton.gameObject.SetActive(!ArrowController.Instance.haveArrow);
        ChangeShootButtonToRecall(ArrowController.Instance.haveArrow);
    }

    public void ChangeShootButtonToRecall(bool haveArrow)
    {
        //maybe be make transition animation
        ShootButton.gameObject.SetActive(haveArrow);
        RecallButton.gameObject.SetActive(!haveArrow);
    }

    void UpdateForceUI(float force)
    {
        forceSlider.gameObject.SetActive(force > 0);
        forceSlider.value = force;
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
        UltimateCanvas.gameObject.SetActive(false);
    }

    public void ShowPlayerUI()
    {
        transform.gameObject.SetActive(true);
        UltimateCanvas.gameObject.SetActive(true);
    }
}