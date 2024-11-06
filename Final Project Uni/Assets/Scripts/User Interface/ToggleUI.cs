using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToggleUIElements : MonoBehaviour
{
    // List to hold all the UI elements you want to toggle
    public List<GameObject> uiElements;

    // Method to toggle the UI elements
    public void ToggleUI(bool isUIEnabled)
    {
        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(isUIEnabled);
        }
    }
}
