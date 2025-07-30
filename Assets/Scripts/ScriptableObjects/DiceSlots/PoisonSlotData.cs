using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PoisonSlotData", menuName = "DiceSlots/PoisonSlotData")]
public class PoisonSlotData : DiceSlotData
{
    public override void Effect(Die slottedDie, int mult, BattleSceneManager sceneManager, Entity owner, DiceSlotController slot)
    {
        List<DiceSlotController> target;
        int repeats = slottedDie.value * mult;
        if (owner is Player)
        {
            target = sceneManager.enemyActiveColumn;
        }
        else
        {
            target = sceneManager.playerActiveColumn;
        }

        for (int i = 0; i < repeats; i++)
        {
            int randomIndex = Random.Range(0, target.Count);
            DiceSlotController targetSlot = target[randomIndex];

            GameObject vfx = Instantiate(vfxPrefab, targetSlot.transform.position, Quaternion.identity);
            vfx.GetComponent<ParticleSystem>()?.Play();

            if (targetSlot.isFilled)
            {
                if (targetSlot.slottedDie.dieTag != "Buff")
                {
                    targetSlot.slottedDie.value -= 1;
                    targetSlot.slottedDie.TranslateValue();
                }
            }
        }
    }
}
