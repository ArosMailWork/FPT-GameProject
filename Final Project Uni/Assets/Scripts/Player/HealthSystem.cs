using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class HealthSystem: MonoBehaviour
{
    [FoldoutGroup("Stats")]public int MaxHealth { get; private set; }
    [FoldoutGroup("Stats")]public int Value       
    {
        get { return value; }
        set
        {
            this.value = value;
            OnValueChanged();
        }
    }
    bool isInvincible = false;
    //float invincibleTime = 0;
    private int value;

    [FoldoutGroup("Debug")] public PlayerController playerController;

    private void Awake()
    {
        MaxHealth = playerController.healthMax;
        Value = MaxHealth;
    }

    private void Start()
    {
        playerController.healthSystem = this;
    }
    
    public delegate void ValueChangeHandler(object sender);
    public event ValueChangeHandler OnValueChange;
    public event ValueChangeHandler OnDeath;
    public event ValueChangeHandler OnInvincible;

    protected virtual void OnValueChanged()
    {
        OnValueChange?.Invoke(this);
    }

    public void ReceiveDamage(int amount)
    {
        if(isInvincible) return;
        Value = Mathf.Max(value - amount, 0);
        if (Value <= 0)
        {
            OnDeath?.Invoke(this);
        }
    }

    public void DoT(float time, int Damage)
    {
        StartCoroutine(DoTTimer(time, Damage));
    }

    private IEnumerator DoTTimer(float time, int Damage)
    {   
        while (true)
        {
            ReceiveDamage(Damage);
            yield return new WaitForSeconds(time);
        }
    }
    
    public void Heal(int amount)
    {
        Value = Mathf.Min(value + amount, MaxHealth);
    }

    public async UniTaskVoid DoInvincible(float time)
    {
        OnDeath?.Invoke(this);

        await InvincibleTimer(time);
    }

    private async UniTask InvincibleTimer(float time)
    {
        isInvincible = true;
        await UniTask.Delay(TimeSpan.FromSeconds(time));
        isInvincible = false;
    }
}