using UnityEngine;

[CreateAssetMenu(fileName = "ScorchedDie_Data", menuName = "Dice/ScorchedDie")]
public class ScorchedDie_Data : DiceData
{
    public override void DoEffect(Die die)
    {
        int damage = 1;
        die.didDamage = damage;
    }

}
