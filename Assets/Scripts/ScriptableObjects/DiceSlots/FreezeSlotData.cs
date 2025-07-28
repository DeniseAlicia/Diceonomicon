using UnityEngine;

[CreateAssetMenu(fileName = "FreezeSlotData", menuName = "DiceSlots/FreezeSlotData")]
public class FreezeSlotData : DiceSlotData
{
    public override void Effect(Die slottedDie, int mult, BattleSceneManager sceneManager, Entity owner, DiceSlotController slot)
    {
        Debug.Log("Dice have been frozen");
    }
}
