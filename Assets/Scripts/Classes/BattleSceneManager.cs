
using UnityEngine;
using System.Collections.Generic;
using System;

public class BattleSceneManager : MonoBehaviour
{
    public Player player;
    public Opponent opponent;
    public List<DiceSlotController> playerActiveColumn;
    public List<DiceSlotController> enemyActiveColumn;
    public int level;

    public void Start()
    {
        Debug.Log("BattleSceneManager.BuildScene");
        opponent.currentHealth = opponent.maxHealth;
        player.currentHealth = player.maxHealth;
    }
    public void PlacementPhase()
    {
        Debug.Log("BattleSceneManager.PlacementPhase");
    }
    public void CalculateDamage()
    {
        player.currentHealth -= Math.Max(opponent.damage - player.block, 0);
        opponent.currentHealth -= Math.Max(player.damage - opponent.block, 0);
    }
    public void EndOfRound()
    {
        ResetEntity(player);
        ResetEntity(opponent);
    }
    private void ResetEntity(Entity entity)
    {
        foreach (Die die in entity.drawnDice)
        {
            entity.drawnDice.Remove(die);
            entity.discardPile.Add(die);
        }

        entity.damage = 0;
        entity.block = 0;
    }

}
