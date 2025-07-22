using UnityEngine;

[CreateAssetMenu(fileName = "ShieldSlotData", menuName = "Scriptable Data/ShieldSlotData")]
public class ShieldSlotData : DiceSlotData
{
    public override void Effect(int dieValue, int mult)
    {
        owner.block += dieValue * mult;  
    }

}
