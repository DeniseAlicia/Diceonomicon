using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "BallteWaypointData", menuName = "Scriptable Objects/BallteWaypointData")]
public class BallteWaypointData : WaypointData
{
    public override void DoEffect()
    {
        SceneManager.LoadScene("BattleSetup", LoadSceneMode.Single);
    }
}