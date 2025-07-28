using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RallySlotData", menuName = "DiceSlots/RallySlotData")]
public class RallySlotData : BuffSlotData
{
    public int bonus;

    public override void Effect(Die slottedDie, int mult, BattleSceneManager sceneManager, Entity owner, DiceSlotController slot)
    {
        List<Die> targetDice = FindTargetDie(slottedDie, slot);
        Debug.Log("Targets: " + string.Join(", ", targetDice));

        foreach (Die targetDie in targetDice)
        {
            Debug.Log($"Value 1: {targetDie.value}");
            targetDie.value += bonus;
            Debug.Log($"Value 2: {targetDie.value}");
        }
    }
}
