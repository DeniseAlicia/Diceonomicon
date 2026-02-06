using UnityEngine;

[CreateAssetMenu(fileName = "CandlemakerWaypointData", menuName = "Scriptable Objects/CandlemakerWaypointData")]
public class CandlemakerWaypointData : WaypointData
{
    public override void DoEffect()
    {
        Debug.Log("No override for DoEffect() found");
    }
}