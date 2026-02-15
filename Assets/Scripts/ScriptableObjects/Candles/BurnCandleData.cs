using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BurnCandleData", menuName = "Scriptable Objects/BurnCandleData")]
public class BurnCandleData : CandleData
{
    public float amount;

    public override void DoEffect()
    {
        int currentHealth = GameStateManager.Instance.player.currentHealth;
        int maxHealth = GameStateManager.Instance.player.maxHealth;

        int damage = (int)Math.Ceiling(maxHealth * amount);

        if (currentHealth - damage > 0)
        {
            GameStateManager.Instance.player.currentHealth -= damage;
        }
        else
        {
           Debug.LogWarning("You Lost!");
        }
    }
}
