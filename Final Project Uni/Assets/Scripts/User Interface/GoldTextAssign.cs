using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldTextAssign : MonoBehaviour
{
    private TextMeshProUGUI text;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        PlayerController.Instance._playerData.GoldText = text;
        text.text = PlayerController.Instance._playerData.Gold.ToString();
    }

}
