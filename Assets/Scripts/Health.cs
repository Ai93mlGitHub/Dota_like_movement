using System;

public class Health
{
    public static event Action<float> HealthChanged;

    public static float MaxHealth = 100f;

    public Health(float defaultValue) => HealthValue = defaultValue;

    public float HealthValue { get; private set; } = 0f;

    public void ChangeHealthValue(float value)
    {
        if ((HealthValue + value) > 0)
            HealthValue += value;
        else
            HealthValue = 0;

        HealthChanged?.Invoke(HealthValue);
    }
}
