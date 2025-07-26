using UnityEngine;

[CreateAssetMenu(fileName = "EmptySlotData", menuName = "DiceSlots/EmptySlotData")]
public class EmptySlotData : DiceSlotData
{
    public override void Effect(int dieValue, int mult, BattleSceneManager sceneManager, Entity owner)
    {
        Debug.Log("Empty Slots cant have effects");
        return;
    }
}
