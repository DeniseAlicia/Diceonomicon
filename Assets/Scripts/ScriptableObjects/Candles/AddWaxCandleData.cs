using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AddWaxCandleData", menuName = "Scriptable Objects/AddWaxCandleData")]
public class AddWaxCandleData : CandleData
{
    public int amount;

    public override void DoEffect()
    {
        GameStateManager.Instance.player.maxHealth += amount;
        GameStateManager.Instance.player.currentHealth += amount;
    }
}
