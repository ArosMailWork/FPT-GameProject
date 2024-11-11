using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    private Slider hpSlider;
    [ReadOnly] public float currentValue;
    [ReadOnly] public float targetValue;
    public float currentVelocity;
    [SerializeField, ReadOnly] private bool isUpdating;

    public bool toggleShow = true;
    [SerializeField] private Image Background;
    [SerializeField] private Image Fill;
    [SerializeField] private float showHideSpeed = 0.1f;

    private Color BGColor;
    private Color FillColor;
    private bool UpdateToggle = true;
    
    private void Awake()
    {
        hpSlider = GetComponent<Slider>();
        targetValue = 1;
        isUpdating = true;

        BGColor = Background.color;
        FillColor = Fill.color;
    }
    private void Update()
    {
        if (isUpdating)
        {
            toggleShow = true;
            currentValue = Mathf.SmoothDamp(currentValue, targetValue, ref currentVelocity, 0.1f);
            hpSlider.value = currentValue;
            if (Mathf.Approximately(currentValue, targetValue)) isUpdating = false;
        }

        if(toggleShow) ShowHPBar();
        else HideHPBar();
        
    }
    public void UpdateHP(float updatedHP, float maxHP)
    {
        float value = updatedHP / maxHP;
        targetValue = value;
        isUpdating = true;
    }

    private void HideHPBar()
    {
        if (Background.color == Color.clear && Fill.color == Color.clear) return;
        //Debug.Log("Hide");
            
        Background.color = Color.Lerp(Background.color, Color.clear, showHideSpeed * 5);
        Fill.color = Color.Lerp(Fill.color, Color.clear, showHideSpeed);
    }
    private void ShowHPBar()
    {
        if (Background.color == BGColor && Fill.color == FillColor) return;
        //Debug.Log("Show");
        
        Fill.color = Color.Lerp(Fill.color, FillColor, showHideSpeed * 5);
        Background.color = Color.Lerp(Background.color, BGColor, showHideSpeed);
    }
}