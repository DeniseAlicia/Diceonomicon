using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RestoreCandleData", menuName = "Scriptable Objects/RestoreCandleData")]
public class RestoreCandleData : CandleData
{
    public float amount;

    public override void DoEffect()
    {
        int currentHealth = GameStateManager.Instance.player.currentHealth;
        int maxHealth = GameStateManager.Instance.player.maxHealth;

        int healing = (int)Math.Ceiling(maxHealth * amount);

        if (currentHealth + healing < maxHealth)
        {
            GameStateManager.Instance.player.currentHealth += healing;
        }
        else
        {
            GameStateManager.Instance.player.currentHealth = maxHealth;
        }
    }
}
