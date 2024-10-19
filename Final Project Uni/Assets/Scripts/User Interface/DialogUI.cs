using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    public Button closeButton;
    public TMP_Text dialogText;
    private bool isTextChanged = false;
    public Mask imageMask;
    
    public event Action OnCloseButtonClicked;

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
        OnCloseButtonClicked?.Invoke();
    }

    void ChangeTextToAllBlackString()
    {
        if (dialogText != null && !isTextChanged)
        {
            int length = dialogText.text.Length;
            dialogText.text = new string('█', length+6);
            isTextChanged = true;
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
            dialogText.fontSize = originalFontSize * 2f; // Reduce font size by 20%
        }
    }
}
