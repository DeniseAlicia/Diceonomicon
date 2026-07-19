using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ChargeSlotData", menuName = "DiceSlots/ChargeSlotData")]
public class ChargeSlotData : DiceSlotData
{
    int health;

    public void Start()
    {
        BattleSceneManager.OnPlacementDone.AddListener(OnPlacementDone);
    }

    public override void Effect(Die slottedDie, int mult, BattleSceneManager sceneManager, Entity owner, DiceSlotController slot)
    {
        if (CheckDamageTaken())
        {
            owner.damage += slottedDie.value * 2 * mult;
        }
    }

    public void OnPlacementDone()
    {
        health = BattleSceneManager.Instance.player.currentHealth;
    }

    public bool CheckDamageTaken()
    {
        if (health != BattleSceneManager.Instance.player.currentHealth)
        {
            return false;
        }
        return true;
    }
}
