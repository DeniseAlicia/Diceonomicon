using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "MimicrySlotData", menuName = "DiceSlots/MimicrySlotData")]
public class MimicrySlotData : DiceSlotData
{
    public override void Effect(Die slottedDie, int mult, Entity owner, DiceSlotController slot)
    {
        List<DiceSlotController> target;
        int triggers = slottedDie.value * mult;
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
                owner.extraDice.Add(targetSlot.slottedDie);
                Debug.Log("Dice have been mimicked");
            }
            else
            {
                Debug.Log("Nothing to mimic");
            }
        }

        if (owner.extraDice.Count > 0)
        {
            BattleSceneManager.Instance.intermission = true;
        }

    }
}
