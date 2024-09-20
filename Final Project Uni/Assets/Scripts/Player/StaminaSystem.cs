using System;
using UnityEngine;

public class StaminaSystem
{
    public int MaxStamina { get; private set; }
    public int RegenRate { get; set; }

    private int value;

    public int Value
    {
        get { return value; }
        set
        {
            if (this.value != value)
            {
                this.value = value;
                // Khi giá trị thay đổi, phát ra sự kiện
                OnValueChanged();
            }
        }
    }
    public StaminaSystem(int maxStamina, int regenRate)
    {
        MaxStamina = maxStamina;
        value = MaxStamina;
        RegenRate = regenRate;
    }

    public delegate void ValueChangeHandler(object sender);
    public event ValueChangeHandler OnValueChange;

    protected virtual void OnValueChanged()
    {
        OnValueChange?.Invoke(this);
    }

    public void Consume(int amount)
    {
        Value = Mathf.Max(value - amount, 0);
    }

    public bool HasEnoughStamina(int amount)
    {
        return Value >= amount;
    }

    public void RegenerateStamina()
    {
        Value = Mathf.Min(Value + Mathf.FloorToInt(RegenRate * Time.deltaTime), MaxStamina);
    }
}