using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PoisonSlotData", menuName = "DiceSlots/PoisonSlotData")]
public class PoisonSlotData : DiceSlotData
{
    public override void Effect(int dieValue, int mult, BattleSceneManager sceneManager, Entity owner)
    {
        List<DiceSlotController> target;
        int repeats = dieValue * mult;
        if (owner is Player)
        {
            target = sceneManager.opponent.activeColumn;
        }
        else
        {
            target = sceneManager.player.activeColumn;
        }

        for (int i = 0; i < repeats; i++)
        {
            int randomIndex = Random.Range(0, target.Count);
            DiceSlotController targetSlot = target[i];
            if (targetSlot.isFilled)
            {
                targetSlot.slottedDie.value -= 1;
            }
        }
    }
}
