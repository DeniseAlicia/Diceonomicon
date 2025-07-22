
using UnityEngine;
using System.Collections.Generic;
using System;

public class BattleSceneManager : MonoBehaviour
{
    public Player player;
    public Opponent opponent;
    public Vector3[] playerPositions;
    public Vector3[] enemyPositions;
    public List<DiceSlotController> playerActiveColumn;
    public List<DiceSlotController> enemyActiveColumn;
    public int level;

    public void BuildScene()
    {
        Debug.Log("BattleSceneManager.BuildScene");
    }
    public void PlacementPhase()
    {
        Debug.Log("BattleSceneManager.PlacementPhase");
    }
    public void CalculateDamage()
    {
        Debug.Log("BattleSceneManager.CalculateDamage");
    }
    public void EndOfRound()
    {
        Debug.Log("BattleSceneManager.EndOfRound");
    }
    private void ResetEntity(Entity _entity)
    {
        Debug.Log("BattleSceneManager.Reset");
    }

}
