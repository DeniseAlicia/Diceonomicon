using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BurnCandleData", menuName = "Scriptable Objects/BurnCandleData")]
public class BurnCandleData : CandleData
{
    public override void DoEffect()
    {
        int currentHealth = GameStateManager.Instance.player.currentHealth;
        int maxHealth = GameStateManager.Instance.player.maxHealth;

        int damage = (int)Math.Ceiling(maxHealth * 0.1);

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
