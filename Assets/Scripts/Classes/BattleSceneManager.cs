
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleSceneManager : MonoBehaviour
{
    public Player player;
    public Opponent opponent;
    public List<DiceSlotController> playerActiveColumn;
    public List<DiceSlotController> enemyActiveColumn;
    public int level;
    private int arbLimit = 10;
    public EndBattle endScene;

    private void Start()
    {
        Debug.Log("BattleSceneManager.BuildScene");
        // opponent.currentHealth = opponent.maxHealth;
        // player.currentHealth = player.maxHealth;

        player.alpha = 0.1f; // 0.1f for rolling, 0.9f for post-placement
        opponent.alpha = 0.1f; // 0.1f for rolling, 0.9f for post-placement
        player.healthText.text = player.currentHealth.ToString();
        opponent.healthText.text = opponent.currentHealth.ToString();

        // Add Test Buttons
        Button upButton = player.healthUp.GetComponent<Button>();
        upButton.onClick.AddListener(GainHealth);

        Button downButton = player.healthDown.GetComponent<Button>();
        downButton.onClick.AddListener(LoseHealth);

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
            // CalculateDamage();
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
        player.healthText.text = player.currentHealth.ToString();

        opponent.currentHealth -= Math.Max(player.damage - opponent.block, 0);
        opponent.healthText.text = player.currentHealth.ToString();

        if (player.currentHealth <= 0)
        {
            endScene.Lose();
        }

        if (opponent.currentHealth <= 0 && player.currentHealth > 0)
        {
            endScene.Win();
        }
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

    public void GainHealth()
    {
        player.currentHealth += 1;
        player.healthText.text = player.currentHealth.ToString();

    }

    public void LoseHealth()
    {
        player.currentHealth -= 1;
        player.healthText.text = player.currentHealth.ToString();

                if (player.currentHealth <= 0)
        {
            endScene.Lose();
        }

        if (opponent.currentHealth <= 0 && player.currentHealth > 0)
        {
            endScene.Win();
        }
    }

}
