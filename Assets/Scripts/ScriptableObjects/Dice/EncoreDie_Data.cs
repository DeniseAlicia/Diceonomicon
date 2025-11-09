using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "EncoreDie_Data", menuName = "Dice/EncoreDie")]
public class EncoreDie_Data : DiceData
{
    public override void DoEffect(Die die)
    {
        //Die[] allDice = FindObjectsByType<Die>(FindObjectsSortMode.None);
        List<Die> allDice = BattleSceneManager.Instance.player.dice;
        foreach (Die dieInstance in allDice)
        {
            if (dieInstance.parentSlot != null && dieInstance.parentSlot.owner.GetType() == typeof(Player) && dieInstance.dieTags.Intersect(dieInstance.dieTags).Any())
            {
                die.value += 1;
                die.TranslateValue();
            }
        }
    }
}
