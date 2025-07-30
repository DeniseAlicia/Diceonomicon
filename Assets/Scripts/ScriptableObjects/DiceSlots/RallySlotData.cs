using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RallySlotData", menuName = "DiceSlots/RallySlotData")]
public class RallySlotData : BuffSlotData
{
    public int bonus;

    public override void Effect(Die slottedDie, int mult, BattleSceneManager sceneManager, Entity owner, DiceSlotController slot)
    {
        List<Die> targetDice = FindTargetDie(slottedDie, slot);

        foreach (Die targetDie in targetDice)
        {
            targetDie.value += bonus;
            targetDie.TranslateValue();
        }
    }
}
