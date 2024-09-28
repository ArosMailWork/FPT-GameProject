using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class StatSliderUI : MonoBehaviour
{
    private Slider hpSlider;
    [ReadOnly] public float currentValue, targetValue;
    public float currentVelocity;
    [SerializeField, ReadOnly] private bool isUpdating;

    public bool toggleShow = true;
    [SerializeField] private Image Background, Fill;

    private Color BGColor, FillColor;
    
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
    }
    
    [Button]
    public void UpdateHP(float updatedHP, float maxHP)
    {
        float value = updatedHP / maxHP;
        targetValue = value;
        isUpdating = true;
    }
}
