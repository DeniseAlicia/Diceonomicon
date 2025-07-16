namespace Diceonomicon

using UnityEngine;

public abstract class Entity: MonoBehaviour
{
    public int Health;
    public Die[] DiceDeck;
    public Die[] DrawnDice;
    public int DrawSize; //how many dice can be drawn at round start
    public Die[] DiscardPile;
    public int Damage = 0;
    public int Block = 0;

    public void DrawDice() {
        Debug.Log("Entity.DrawDice");
    }
    public void RollDice(){
        Debug.Log("Entity.RollDice");
    }

    public void CalculateColumns(){
        Debug.Log("Entity.CalculateColumns")
    }

}