using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "StoryWaypointData", menuName = "Scriptable Objects/StoryWaypointData")]
public class StoryWaypointData : WaypointData
{
    public override void DoEffect()
    {
        //SceneManager.LoadScene("BattleSetup", LoadSceneMode.Single);
    }
}