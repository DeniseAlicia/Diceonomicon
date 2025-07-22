using UnityEngine;

[CreateAssetMenu(fileName = "ShieldSlotData", menuName = "Data/ShieldSlotData")]
public class ShieldSlotData : DiceSlotData
{
    public override void Effect(int dieValue, int mult)
    {
        owner.block += dieValue * mult;  
    }

}
