using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEdit : MonoBehaviour
{
    public int requireKnowledgeLevel;
    
    [TextArea] public string text;
    public float fontSize = -1;
    public bool Mask;
    
    public void ReadDialogue()
    {
        ToggleDialogue();
        EditText();
    }

    public void ToggleMask(bool toggle)
    {
        Mask = toggle;
    }
    public void EditText()
    {
        PlayerController.Instance.dialogUI.ChangeText(text, fontSize);
        
        //Check Player Knowledge Level to read
        if (requireKnowledgeLevel <= PlayerController.Instance._stats.knowledgeLevel)
            PlayerController.Instance.dialogUI.ShowOriginalText();
        else
            PlayerController.Instance.dialogUI.ChangeTextFontHidden();
        
        //Toggle Mask
        PlayerController.Instance.dialogUI.SetActiveMask(Mask);
    }
    
    public void ToggleDialogue()
    {
        PlayerController.Instance.dialogUI.toggleScale.ScaleToggle();
    }
    public void ForcedDownDialogue()
    {
        PlayerController.Instance.dialogUI.toggleScale.ForceScaleDown();
    }
}
