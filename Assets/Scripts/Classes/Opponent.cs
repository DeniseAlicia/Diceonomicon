
using UnityEngine;
using System.Collections.Generic;

public class Opponent : Entity
{
    public EnemyAI ai;
    public List<TabletData> ActiveImplings;

    public void SetEnemyRoster(List<TabletData> army)
    {
        maxHealth = 0;

        ActiveImplings = army;

        foreach (TabletData demon in army)
        {
            for (int i = 0; i < demon.startingDice.Length; i++)
            {
                diceDeck.Add(demon.startingDice[i]);
            }
        }
    }

    public override void RollDice()
    {
        return;
    }

    public override void SetHealth()
    {
        foreach (TabletData imp in ActiveImplings)
        {
            maxHealth += imp.health;
        }
        currentHealth = maxHealth;
    }
}
