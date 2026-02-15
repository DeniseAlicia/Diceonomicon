using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "PuzzleWaypointData", menuName = "Scriptable Objects/PuzzleWaypointData")]
public class PuzzleWaypointData : WaypointData
{
    public override void DoEffect()
    {
        //SceneManager.LoadScene("BattleSetup", LoadSceneMode.Single);
    }
}