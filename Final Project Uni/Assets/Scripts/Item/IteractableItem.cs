using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class IteractableItem : MonoBehaviour
{
    [SerializeField]
    private float distanceActiveInteraction = 2f;

    [SerializeField]
    private float deactivationDelay = 1f;

    [SerializeField]
    private bool isActive = true;

    public UnityEvent OnTouch;

    private GameObject dialog;
    private ScaleEffect scaleEffect;

    private void Awake()
    {
        dialog = transform.GetChild(0).gameObject;
        scaleEffect = dialog.GetComponent<ScaleEffect>();
        scaleEffect.OnScaleDownComplete += OnScaleDownComplete;
    }

    private void Start()
    {
        OnTouch.AddListener(ToggleDialog);
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

    private void OnScaleDownComplete()
    {
        // This method is no longer needed, but kept for compatibility
    }

    private void OnMouseDown()
    {
        if (isActive)
        {
            OnTouch.Invoke();
        }
    }

    private void OnDestroy()
    {
        if (scaleEffect != null)
        {
            scaleEffect.OnScaleDownComplete -= OnScaleDownComplete;
        }
    }

    // Uncomment and modify this method if you want to implement distance-based activation
    /*
    private void Update()
    {
        isActive = Vector3.Distance(transform.position, Player.Instance.transform.position) <= distanceActiveInteraction;
    }
    */
}
