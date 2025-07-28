
using UnityEngine;
using System.Collections.Generic;

public class Opponent : Entity
{
    public TabletData[] army;
    public EnemyAI ai;


    public void SetEnemyRoster()
    {
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

}
