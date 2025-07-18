namespace Diceonomicon
{
    using UnityEngine;
    using System.Collections.Generic;
    using System;

    public class BattleScene : MonoBehaviour
    {
        public Player player;
        public Opponent opponent;
        public Vector3[] playerPositions;
        public Vector3[] enemyPositions;
        public List<DiceSlot> playerActiveColumn;
        public List<DiceSlot> enemyActiveColumn;
        public int level;
        public event Action RoundStart;

        private void Awake()
        {
            Debug.Log("Hello there");
            player.Test();
        }

        private void Update()
        {
            RoundStart?.Invoke();
        }
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
        private void ResetEntity(Entity _entity)
        {
            Debug.Log("BattleScene.Reset");
        }

    }
}