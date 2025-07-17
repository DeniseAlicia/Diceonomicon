namespace Diceonomicon
{
    using UnityEngine;

    public class BattleScene
    {
        public Player Player;
        public Enemy[] Enemies;
        public Vector3[] PlayerPositions;
        public Vector3[] EnemyPositions;
        public DiceSlot[] PlayerActiveColumn;
        public DiceSlot[] EnemyActiveColumn;
        public int Level;

        public void BuildScene()
        {
            Debug.Log("BattleScene.BuildScene");
        }
        public void PlacementPhase()
        {
            Debug.Log("BattleScene.PlacementPhase");
        }
        public void CalculateDamage()
        {
            Debug.Log("BattleScene.CalculateDamage");
        }
        public void EndOfRound()
        {
            Debug.Log("BattleScene.EndOfRound");
        }
        private void ResetEntity(Entity _Entity)
        {
            Debug.Log("BattleScene.Reset");
        }

    }
}