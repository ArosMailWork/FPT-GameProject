using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    public Button closeButton;
    public TMP_Text dialogText;
    public Mask imageMask;

    private float originalFontSize;

    void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(OnCloseButtonClick);
        }
        else
        {
            Debug.LogWarning("Close button is not assigned in DialogUI.");
        }

        if (imageMask != null)
        {
            imageMask.enabled = false;
        }
        else
        {
            Debug.LogWarning("Image mask is not assigned in DialogUI.");
        }

        if (dialogText != null)
        {
            originalFontSize = dialogText.fontSize;
        }
    }

    void OnCloseButtonClick()
    {
        ChangeTextToAllBlackString();
        if (imageMask != null)
        {
            imageMask.enabled = true;
        }
        ChangeFontSize();
    }

    void ChangeTextToAllBlackString()
    {
        if (dialogText != null)
        {
            int length = dialogText.text.Length;
            dialogText.text = new string('█', length);
        }
        else
        {
            Debug.LogWarning("Dialog text is not assigned in DialogUI.");
        }
    }

    void ChangeFontSize()
    {
        if (dialogText != null)
        {
            dialogText.fontSize = originalFontSize * 0.8f; // Reduce font size by 20%
        }
    }
}
