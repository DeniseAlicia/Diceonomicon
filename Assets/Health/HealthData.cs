using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Data/Health Data")]
public class HealthData : ScriptableObject
{
    public int currentHealth = 100;

    public event Action OnValueChanged;

    public void DamageTaken(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        OnValueChanged?.Invoke();
    }
}
