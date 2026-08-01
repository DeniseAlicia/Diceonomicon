using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EncoreDie_Data", menuName = "Dice/EncoreDie")]
public class EncoreDie_Data : DiceData
{
    public override void DoEffect(Die die)
    {
        //Die[] allDice = FindObjectsByType<Die>(FindObjectsSortMode.None);
        List<Die> allDice = Player.Instance.dice;
        foreach (Die dieInstance in allDice)
        {
            if (dieInstance.parentSlot != null && dieInstance.parentSlot.owner.GetType() == typeof(Player) && dieInstance.dieTags.Intersect(dieInstance.dieTags).Any())
            {
                die.value += 1;
                DieAction.UpdateText(die);
            }
        }
    }
}