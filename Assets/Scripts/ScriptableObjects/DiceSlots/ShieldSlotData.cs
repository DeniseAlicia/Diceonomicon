using UnityEngine;

[CreateAssetMenu(fileName = "ShieldSlotData", menuName = "DiceSlots/ShieldSlotData")]
public class ShieldSlotData : DiceSlotData
{
    public override void Effect(int dieValue, int mult, BattleSceneManager sceneManager, Entity owner)
    {
        owner.block += dieValue * mult;  
    }

}
