using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class DialogUI : MonoBehaviour
{
    public TMP_Text dialogText;
    public TMP_FontAsset alternateFont;
    private bool isFontChanged = false;
    public Mask imageMask;
    [CanBeNull] public ScaleEffect toggleScale;

    //Store stuffs
    float originalFontSize;
    TMP_FontAsset originalFont;

    void Start()
    {
        SetActiveMask(false);
        if (dialogText != null)
        {
            originalFontSize = dialogText.fontSize;
            originalFont = dialogText.font; // Store the original font
        }
    }

    public void SetActiveMask(bool isActive)
    {
        if (imageMask != null)
            imageMask.enabled = isActive;
    }

    [Button]
    public void ChangeTextFontHidden()
    {
        if (dialogText != null && !isFontChanged && alternateFont != null)
        {
            dialogText.font = alternateFont; // Change to the alternate font
            isFontChanged = true;
        }
        else if (alternateFont == null)
        {
            Debug.Log("Alternate font is not assigned.");
        }
    }
    [Button]
    public void ShowOriginalText()
    {
        if (dialogText != null && isFontChanged)
        {
            dialogText.font = originalFont; // Restore the original font
            isFontChanged = false;
        }
    }

    [Button]
    public void ChangeText(string input, float fontSize = -1)
    {
        dialogText.text = input;
        if (fontSize >= 0) 
            dialogText.fontSize = fontSize;
        else
            dialogText.fontSize = originalFontSize;
    }

    public void changeToggle()
    {
        if(toggleScale == null) return;
        toggleScale.ScaleToggle();
    }
    
    public void ForcedToggleDown()
    {
        if(toggleScale == null) return;
        toggleScale.ForceScaleDown();
    }
}
