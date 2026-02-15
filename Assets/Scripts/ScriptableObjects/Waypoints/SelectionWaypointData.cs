using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SelectionWaypointData", menuName = "Scriptable Objects/SelectionWaypointData")]
public class SelectionWaypointData : WaypointData
{
    public override void DoEffect()
    {
        SceneManager.LoadScene("RewardSelection", LoadSceneMode.Additive);
    }
}