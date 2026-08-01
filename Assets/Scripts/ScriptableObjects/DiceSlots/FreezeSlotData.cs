using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FreezeSlotData", menuName = "DiceSlots/FreezeSlotData")]
public class FreezeSlotData : BuffSlotData
{
    public override void Effect(Die slottedDie, int mult, Entity owner, DiceSlotController slot)
    {
        List<Die> targetDice = FindTargetDie(slottedDie, slot);

        foreach (Die targetDie in targetDice)
        {
            targetDie.isFrozen = true;
            owner.drawSize -= 1;
        }
    }
}
