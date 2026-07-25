using UnityEngine;

[CreateAssetMenu(fileName = "BallteWaypointData", menuName = "Scriptable Objects/BallteWaypointData")]
public class BallteWaypointData : WaypointData
{
    public override void DoEffect()
    {
        SceneTransition.Load("BattleSetup");
    }
}