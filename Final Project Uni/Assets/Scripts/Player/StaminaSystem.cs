namespace PA
{
    public class StaminaSystem
    {
        public float MaxStamina { get; private set; }
        public float RegenRate { get; set; }


        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    // Khi giá trị thay đổi, phát ra sự kiện
                    OnValueChanged(EventArgs.Empty);
                }
            }
        }
        public StaminaSystem(float maxStamina, float regenRate)
        {
            MaxStamina = maxStamina;
            Value = MaxStamina;
            RegenRate = regenRate;
        }
        public delegate void ValueChangedEventHandler(object sender, EventArgs e);
        public event ValueChangedEventHandler OnValueChange;
        protected virtual void OnValueChanged(EventArgs e)
        {
            // Kiểm tra xem có ai đăng ký lắng nghe sự kiện chưa
            if (OnValueChange != null)
            {
                OnValueChange(this, e);
            }
        }

        public bool ConsumeStamina(PlayerAction action)
        {
            switch (action)
            {
                case PlayerAction.Run:
                    if (HasEnoughStamina(10))
                    {
                        SubtractStamina(10);
                        return true;
                    }
                    return false;
                case PlayerAction.Attack:
                    if (HasEnoughStamina(20))
                    {
                        SubtractStamina(20);
                        return true;
                    }
                    return false;
                default:
                    return false;
            }
        }

        public void SubtractStamina(float amount)
        {
            CurrentStamina = Mathf.Max(CurrentStamina - amount, 0);
        }
        public bool HasEnoughStamina(float amount)
        {
            return Value >= amount;
        }

        public void RegenerateStamina()
        {
            CurrentStamina = Mathf.Min(CurrentStamina + RegenRate * Time.deltaTime, MaxStamina);
        }

    }

}