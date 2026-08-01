using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "PoisonSlotData", menuName = "DiceSlots/PoisonSlotData")]
public class PoisonSlotData : DiceSlotData
{
    public override void Effect(Die slottedDie, int mult, Entity owner, DiceSlotController slot)
    {
        affectedDice = 0;
        List<DiceSlotController> target;
        int triggers = slottedDie.value * mult;
        multipliedValue = triggers;
        if (owner is Player)
        {
            target = BattleSceneManager.Instance.enemyActiveColumn;
        }
        else
        {
            target = BattleSceneManager.Instance.playerActiveColumn;
        }

        for (int i = 0; i < triggers; i++)
        {
            int randomIndex = Random.Range(0, target.Count);
            DiceSlotController targetSlot = target[randomIndex];

            GameObject vfx = Instantiate(vfxPrefab, targetSlot.transform.position, Quaternion.identity);
            vfx.GetComponent<ParticleSystem>()?.Play();

            if (targetSlot.isFilled)
            {
                if (!targetSlot.slottedDie.dieTags.Contains("Buff")) 
                {
                    targetSlot.slottedDie.value -= 1;
                    DieAction.UpdateText(targetSlot.slottedDie);
                    affectedDice++;
                }
            }
        }
    }
}
