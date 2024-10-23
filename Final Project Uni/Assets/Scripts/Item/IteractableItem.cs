using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractableItem : MonoBehaviour
{
    public GameObject dialog;
    public Button interactButton;

    // New boolean to control if UI interaction is one-time only
    public bool oneTimeUseUI = false;

    // Unity events for interaction and trigger range handling
    public UnityEvent InteractEvent, EnterTriggerRange, ExitTriggerRange;

    // Track if the interaction has occurred when oneTimeUseUI is true
    private bool hasInteracted = false;

    private void Awake()
    {
        interactButton.gameObject.SetActive(false);
        interactButton.onClick.AddListener(() => OnInteract());
    }

    // Method to be called when the interact button is clicked
    public void OnInteract()
    {
        if (oneTimeUseUI && hasInteracted) return;

        InteractEvent.Invoke();

        if (oneTimeUseUI)
        {
            hasInteracted = true;  // Mark as interacted
            interactButton.gameObject.SetActive(false);  // Hide the button after interaction
        }
    }

    // Method to show/hide the interact UI
    public void ShowUIInteract(bool toggle)
    {
        if (!hasInteracted || !oneTimeUseUI)  // Only show the button if interaction hasn't happened (for one-time use)
        {
            interactButton.gameObject.SetActive(toggle);
        }
    }

    // Called when a player enters the interaction range
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        EnterTriggerRange.Invoke();
        ShowUIInteract(true);
    }

    // Called when a player leaves the interaction range
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        ExitTriggerRange.Invoke();
        ShowUIInteract(false);
    }
}
