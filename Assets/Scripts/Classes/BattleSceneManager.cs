
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
    private int arbLimit = 10;

    private void Start()
    {
        Debug.Log("BattleSceneManager.BuildScene");
        opponent.currentHealth = opponent.maxHealth;
        player.currentHealth = player.maxHealth;
        NewRound();
    }

    private void NewRound()
    {
        if (arbLimit > 0)
        {
            opponent.DrawDice();
            opponent.RollDice();
            //opponent.ai.PlaceDice(opponent.drawnDice);
            PlacementPhase();
            CombatManager.HandleActiveCombat(this);
            CalculateDamage();
            EndOfRound();
        }
    }
    private void PlacementPhase()
    {
        player.DrawDice();
        player.RollDice();
    }
    private void CalculateDamage()
    {
        player.currentHealth -= Math.Max(opponent.damage - player.block, 0);
        opponent.currentHealth -= Math.Max(player.damage - opponent.block, 0);
    }
    private void EndOfRound()
    {
        ResetEntity(player);
        ResetEntity(opponent);
        arbLimit -= 1;
        NewRound();
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
