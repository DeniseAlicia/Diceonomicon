public class Magician : Trait
{
    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "Spell dice gain a +1 bonus and Damage dice get a -1 penalty";
        tablet.descText.text = description;

        BattleSceneManager.OnPlacementDone.AddListener(OnPlacementDone);
        BattleSceneManager.OnAcvitveCombatStart.AddListener(OnAcvitveCombatStart);
    }

    public void OnPlacementDone()
    {
        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slotTag == "Spell")
            {
                if (slot.slottedDie != null && slot.slottedDie.isCopy)
                {
                    slot.slottedDie.value += 1;
                    DieAction.UpdateText(slot.slottedDie);
                }
            }
        }

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slotTag == "Damage")
            {
                if (slot.slottedDie != null && slot.slottedDie.isCopy)
                {
                    slot.slottedDie.value -= 1;
                    DieAction.UpdateText(slot.slottedDie);
                }
            }
        }
    }

    public void OnAcvitveCombatStart()
    {
        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slotTag == "Spell")
            {
                if (slot.slottedDie != null)
                {
                    slot.slottedDie.value += 1;
                    DieAction.UpdateText(slot.slottedDie);
                }
            }
        }

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slotTag == "Damage")
            {
                if (slot.slottedDie != null)
                {
                    slot.slottedDie.value -= 1;
                    DieAction.UpdateText(slot.slottedDie);
                }
            }
        }
    }

    public override void UnsubscribeFromEvents()
    {
        BattleSceneManager.OnPlacementDone.RemoveListener(OnPlacementDone);
        BattleSceneManager.OnAcvitveCombatStart.RemoveListener(OnAcvitveCombatStart);
    }
}