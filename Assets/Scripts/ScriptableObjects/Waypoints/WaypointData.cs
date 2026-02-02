using UnityEngine;

[CreateAssetMenu(fileName = "WaypointData", menuName = "Scriptable Objects/WaypointData")]
public class WaypointData : ScriptableObject
{
    public string type; // Item, Dice, Slot, Healing, Battle, etc...
    public string subType; // Random, Pick 1 (out of 3), etc...
    public string desc;
    public Texture waypointArt;
    public Texture closedPathArt;

    public virtual void DoEffect()
    {
        Debug.Log("No override for DoEffect() found");
    }
}