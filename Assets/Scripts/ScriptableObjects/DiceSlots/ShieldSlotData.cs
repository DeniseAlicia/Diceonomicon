using UnityEngine;

[CreateAssetMenu(fileName = "ShieldSlotData", menuName = "DiceSlots/ShieldSlotData")]
public class ShieldSlotData : DiceSlotData
{
    public override void Effect(Die slottedDie, int mult, BattleSceneManager sceneManager, Entity owner, DiceSlotController slot)
    {
        owner.block += slottedDie.value * mult;  
    }

}
