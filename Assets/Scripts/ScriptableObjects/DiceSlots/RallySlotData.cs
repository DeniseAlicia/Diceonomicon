using UnityEngine;

[CreateAssetMenu(fileName = "RallySlotData", menuName = "DiceSlots/RallySlotData")]
public class RallySlotData : DiceSlotData
{
    public override void Effect(int dieValue, int mult, BattleSceneManager sceneManager, Entity owner)
    {
        Debug.Log("Dice have been rallied");
    }
}
