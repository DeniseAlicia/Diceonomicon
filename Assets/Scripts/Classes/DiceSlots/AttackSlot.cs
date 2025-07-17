namespace Diceonomicon
{
    using UnityEngine;

    public class AttackSlot : CombatSlot
    {
        public override void DoEffect()
        {
            int DamageOutput = this.SlottedDie.Value * this.Mult;

        }
    }
}