using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class DialogUI : MonoBehaviour
{
    public int requireKnowledgeLevel;
    
    public TMP_Text dialogText;
    public TMP_FontAsset alternateFont;
    private bool isFontChanged = false;
    public Mask imageMask;

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

        if (requireKnowledgeLevel <= PlayerController.Instance._stats.knowledgeLevel)
            ShowOriginalText();
        else
            ChangeTextFontHidden();
    }

    void SetActiveMask(bool isActive)
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
            Debug.LogWarning("Alternate font is not assigned.");
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
}
