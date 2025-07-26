using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MimicrySlotData", menuName = "DiceSlots/MimicrySlotData")]
public class MimicrySlotData : DiceSlotData
{
    public override void Effect(int dieValue, int mult, BattleSceneManager sceneManager, Entity owner)
    {
        List<DiceSlotController> target = owner.activeColumn;
        int repeats = dieValue * mult;
        for (int i = 0; i < repeats; i++)
        {
            int randomIndex = Random.Range(0, target.Count);
            DiceSlotController targetSlot = target[randomIndex];
        }
        Debug.Log("Dice have been mimicked");
    }
}
