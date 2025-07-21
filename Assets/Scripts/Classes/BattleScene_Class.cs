
using UnityEngine;
using System.Collections.Generic;
using System;

public class BattleScene : MonoBehaviour
{
    public Player player;
    public Opponent opponent;
    public Vector3[] playerPositions;
    public Vector3[] enemyPositions;
    public List<DiceSlotData> playerActiveColumn;
    public List<DiceSlotData> enemyActiveColumn;
    public int level;

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
