using UnityEngine;

[CreateAssetMenu(fileName = "FreezeSlotData", menuName = "DiceSlots/FreezeSlotData")]
public class FreezeSlotData : DiceSlotData
{
    public override void Effect(int dieValue, int mult, BattleSceneManager sceneManager, Entity owner)
    {
        Debug.Log("Dice have been frozen");
    }
}
