using System;

public class Thorns : Trait
{
    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "Adds half of the Block value to the damage";
        tablet.descText.text = description;

        BattleSceneManager.OnSlotTriggered += OnSlotTriggered;
    }

    public void OnSlotTriggered(DiceSlotController slot)
    {
        if (slot.slottedDie != null && tablet.tabletSlots.Contains(slot) && slot.tag == "Block")
        {
            if (slot.owner == Player.Instance)
            {
                Player.Instance.damage += (int)Math.Ceiling((double)slot.slottedDie.value / 2);
            }
            else
            {
                Opponent.Instance.damage += (int)Math.Ceiling((double)slot.slottedDie.value / 2);
            }

        }
    }

    public override void UnsubscribeFromEvents()
    {
        BattleSceneManager.OnSlotTriggered -= OnSlotTriggered;
    }
}