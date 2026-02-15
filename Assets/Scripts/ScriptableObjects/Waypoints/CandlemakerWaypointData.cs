using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "CandlemakerWaypointData", menuName = "Scriptable Objects/CandlemakerWaypointData")]
public class CandlemakerWaypointData : WaypointData
{
    public override void DoEffect()
    {
        SceneManager.LoadScene("RewardSelection", LoadSceneMode.Additive);
    }
}