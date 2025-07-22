using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AttackSlotData", menuName = "Data/AttackSlotData")]
public class AttackSlotData : DiceSlotData
{
    public override void Effect(int dieValue, int mult)
    {
        owner.damage += dieValue * mult;
    }
}
