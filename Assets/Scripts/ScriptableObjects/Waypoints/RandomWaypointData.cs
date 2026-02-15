using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "RandomWaypointData", menuName = "Scriptable Objects/RandomWaypointData")]
public class RandomWaypointData : WaypointData
{
    public override void DoEffect()
    {
        SceneManager.LoadScene("RewardSelection", LoadSceneMode.Additive);
    }
}