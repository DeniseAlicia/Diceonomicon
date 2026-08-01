using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GhostlyAttackSlotData", menuName = "DiceSlots/GhostlyAttackSlotData")]
public class GhostlyAttackSlotData : DiceSlotData
{
    public override void Effect(Die slottedDie, int mult, Entity owner, DiceSlotController slot)
    {
        owner.unblockableDamage += (int)Math.Round(slottedDie.value * mult / 2.0);
    }
}
