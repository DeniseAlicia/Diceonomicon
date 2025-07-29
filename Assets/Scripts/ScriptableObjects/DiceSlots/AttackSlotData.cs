using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AttackSlotData", menuName = "DiceSlots/AttackSlotData")]
public class AttackSlotData : DiceSlotData
{
    public override void Effect(Die slottedDie, int mult, BattleSceneManager sceneManager, Entity owner, DiceSlotController slot)
    {
            owner.damage += slottedDie.value * mult;
    }
}
