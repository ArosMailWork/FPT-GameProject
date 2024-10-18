using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IteractableItem : MonoBehaviour
{
    public GameObject dialog;
    public Button activeButton;

    [SerializeField]
    private float distanceActiveInteraction = 2f;

    [SerializeField]
    private float deactivationDelay = 1f;

    [SerializeField]
    private bool isActive = true;

    public UnityEvent OnTouch;

    private ScaleEffect scaleEffect;

    private void Awake()
    {
        dialog.SetActive(false);
        activeButton.gameObject.SetActive(false);
        scaleEffect = dialog.GetComponent<ScaleEffect>();
        scaleEffect.OnScaleDownComplete += OnScaleDownComplete;
        scaleEffect.OnScaleUpComplete += OnScaleUpComplete;
    }

    private void Start()
    {
        OnTouch.AddListener(ToggleDialog);
        activeButton.onClick.AddListener(OnActiveButtonClicked);
    }

    public void OnActiveButtonClicked()
    {
        OnTouch.Invoke();
    }

    public async void ToggleDialog()
    {
        if (dialog.activeSelf)
        {
            await scaleEffect.ScaleDownAsync();
            dialog.SetActive(false);
        }
        else
        {
            dialog.SetActive(true);
            await scaleEffect.ScaleUpAsync();
        }
    }

    // private void OnMouseDown()
    // {
    //     if (isActive)
    //     {
    //         OnTouch.Invoke();
    //     }
    // }

    private void OnDestroy()
    {
        if (scaleEffect != null)
        {
            scaleEffect.OnScaleDownComplete -= OnScaleDownComplete;
            scaleEffect.OnScaleUpComplete -= OnScaleUpComplete;
        }
    }

    private void OnScaleUpComplete()
    {
        activeButton.gameObject.SetActive(false);
    }

    private void OnScaleDownComplete()
    {
        activeButton.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter called with: {other.gameObject.name}");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player tag detected");
            isActive = true;
            activeButton.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log($"Collider entered, but not player. Tag: {other.tag}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isActive = false;
            activeButton.gameObject.SetActive(false);
        }
    }
}
