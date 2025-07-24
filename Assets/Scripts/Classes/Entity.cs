
using UnityEngine;
using System.Collections.Generic;

public abstract class Entity : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public List<Die> diceDeck;
    public List<Die> drawnDice;
    public int drawSize; //how many dice can be drawn at round start
    public List<Die> discardPile;
    public int damage = 0;
    public int block = 0;
    public List<DiceSlotController> activeColumn;

    public void DrawDice()
    {
        for (int i = 0; i < drawSize; i++)
        {
            if (diceDeck == null)
            {
                if (discardPile == null)
                {
                    return;
                }
                foreach (Die die in discardPile)
                    {
                        discardPile.Remove(die);
                        diceDeck.Add(die);
                    }
            }
            int randomIndex = Random.Range(0, diceDeck.Count);
            Die drawnDie = diceDeck[randomIndex];
            diceDeck.Remove(drawnDie);
            drawnDice.Add(drawnDie);
        }
    }
    public void RollDice()
    {
        foreach (Die die in drawnDice)
        {
            die.Roll();
        }
    }

    public void CalculateColumns()
    {
        Debug.Log("Entity.CalculateColumns");
    }


}