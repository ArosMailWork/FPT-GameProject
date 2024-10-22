using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class IteractableItem : MonoBehaviour
{
    public GameObject dialog;
    public Button activeButton;
    public int requireKnowledgeLevel;

    [SerializeField]
    //only need active dialog one time
    private bool isOpen = false;

    public UnityEvent OnTouch;

    private ScaleEffect scaleEffect;

    private void Awake()
    {
        dialog.SetActive(false);
        activeButton.gameObject.SetActive(false);
        scaleEffect = dialog.GetComponent<ScaleEffect>();
        var dialogUI = dialog.GetComponent<DialogUI>();
        dialogUI.OnCloseButtonClicked += ChangeObjectToOpened;
        scaleEffect.OnScaleDownComplete += OnScaleDownComplete;
        scaleEffect.OnScaleUpComplete += OnScaleUpComplete;
    }

    private void ChangeObjectToOpened()
    {
        isOpen = true;
    }

    public void Start()
    {
        OnTouch.AddListener(ToggleDialog);
        activeButton.onClick.AddListener(OnActiveButtonClicked);
    }

    public virtual void OnActiveButtonClicked()
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
        if (other.CompareTag("Player"))
        {
            if (isOpen == true)
                return;
            activeButton.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            activeButton.gameObject.SetActive(false);
        }
    }
}
