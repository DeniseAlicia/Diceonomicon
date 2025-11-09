using UnityEngine;

[CreateAssetMenu(fileName = "CrackedDie_Data", menuName = "Dice/CrackedDie")]
public class CrackedDie_Data : DiceData
{
    public override void DoEffect(Die die)
    {
        Entity self = die.parentSlot.owner;
        self.drawnDice.Remove(die.data);
    }
}
