namespace Diceonomicon
{
    using UnityEngine;

    public class AttackSlot : CombatSlot
    {
        public override void DoEffect()
        {
            int damageOutput = slottedDie.value * mult;
            if (slottedDie.owner == "Player")
            {
            
            }
            else
            {
               
            }

        }


    }
}