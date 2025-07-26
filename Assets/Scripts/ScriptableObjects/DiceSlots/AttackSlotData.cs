using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AttackSlotData", menuName = "DiceSlots/AttackSlotData")]
public class AttackSlotData : DiceSlotData
{
    public override void Effect(int dieValue, int mult, BattleSceneManager sceneManager, Entity owner)
    {
        owner.damage += dieValue * mult;
    }
}
