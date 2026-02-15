using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "KeyWaypointData", menuName = "Scriptable Objects/KeyWaypointData")]
public class KeyWaypointData : WaypointData
{
    public override void DoEffect()
    {
        SceneManager.LoadScene("RewardSelection", LoadSceneMode.Additive);
    }
}