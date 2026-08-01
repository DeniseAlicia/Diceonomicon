using UnityEngine;

[CreateAssetMenu(fileName = "AttackSlotData", menuName = "DiceSlots/AttackSlotData")]
public class AttackSlotData : DiceSlotData
{
    public override void Effect(Die slottedDie, int mult, Entity owner, DiceSlotController slot)
    {
            owner.damage += slottedDie.value * mult;
    }
}
