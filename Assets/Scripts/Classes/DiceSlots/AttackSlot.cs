namespace Diceonomicon
{
    using UnityEngine;

    public class AttackSlot : CombatSlot
    {
        public override void DoEffect()
        {
            int damageOutput = this.slottedDie.Value * this.mult;

        }
    }
}