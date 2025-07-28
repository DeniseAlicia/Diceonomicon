using UnityEngine;

[CreateAssetMenu(fileName = "EmptySlotData", menuName = "DiceSlots/EmptySlotData")]
public class EmptySlotData : DiceSlotData
{
    public override void Effect(Die slottedDie, int mult, BattleSceneManager sceneManager, Entity owner, DiceSlotController slot)
    {
        Debug.Log("Empty Slots cant have effects");
        return;
    }
}
