using UnityEngine;

[CreateAssetMenu(fileName = "FreezeSlotData", menuName = "Data/FreezeSlotData")]
public class FreezeSlotData : DiceSlotData
{
    public override void Effect(int dieValue, int mult)
    {
        Debug.Log("Dice have been frozen");
    }
}
