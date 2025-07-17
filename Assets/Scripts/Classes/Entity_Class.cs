namespace Diceonomicon
{
    using UnityEngine;

    public abstract class Entity : MonoBehaviour
    {
        public int health;
        public Die[] diceDeck;
        public Die[] drawnDice;
        public int drawSize; //how many dice can be drawn at round start
        public Die[] discardPile;
        public int damage = 0;
        public int block = 0;

        public void DrawDice() {
            Debug.Log("Entity.DrawDice");
        }
        public void RollDice() {
            Debug.Log("Entity.RollDice");
        }

        public void CalculateColumns() {
            Debug.Log("Entity.CalculateColumns");
        }

    }
}