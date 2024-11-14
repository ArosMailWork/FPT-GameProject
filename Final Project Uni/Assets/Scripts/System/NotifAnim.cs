using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotifAnim : MonoBehaviour
{
    public TMP_Text notifText;
    public Animator NotifTextAnimator;

    public void Notif(string input)
    {
        EditText(input);
        Show();
    }
    
    void EditText(string input)
    {
        notifText.text = input;
    }
    public void Show()
    {
        NotifTextAnimator.SetTrigger("Show");
    }
}
