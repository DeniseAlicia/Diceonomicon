namespace Diceonomicon
{
    using UnityEngine;

    public class AttackSlot : CombatSlot
    {
        public override void DoEffect()
        {
            int damageOutput = this.slottedDie.value * this.mult;
            if (this.slottedDie.owner == "Player")
            {
            
            }
            else
            {
               
            }

        }


    }
}