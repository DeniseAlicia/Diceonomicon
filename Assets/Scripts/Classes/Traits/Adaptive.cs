public class Adaptive : Trait
{
    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "Damage dice gain a +1 bonus and Block dice get a -1 penalty";
        tablet.descText.text = description;

        BattleSceneManager.OnPlacementDone.AddListener(OnPlacementDone);
        BattleSceneManager.OnAcvitveCombatStart.AddListener(OnAcvitveCombatStart);
    }


    public void OnPlacementDone()
    {
        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slotTag == "Block")
            {
                if (slot.slottedDie != null && slot.slottedDie.isCopy)
                {
                    if (Player.Instance.currentHealth < Player.Instance.maxHealth / 2)
                    {
                        slot.slottedDie.value += 1;
                        DieAction.UpdateText(slot.slottedDie);
                    }
                    else
                    {
                        slot.slottedDie.value -= 1;
                        DieAction.UpdateText(slot.slottedDie);
                    }
                }
            }
        }

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slotTag == "Damage")
            {
                if (slot.slottedDie != null && slot.slottedDie.isCopy)
                {
                    if (Player.Instance.currentHealth < Player.Instance.maxHealth / 2)
                    {
                        slot.slottedDie.value -= 1;
                        DieAction.UpdateText(slot.slottedDie);
                    }
                    else
                    {
                        slot.slottedDie.value += 1;
                        DieAction.UpdateText(slot.slottedDie);
                    }
                }
            }
        }
    }

    public void OnAcvitveCombatStart()
    {
        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slotTag == "Block")
            {
                if (slot.slottedDie != null)
                {
                    if (Player.Instance.currentHealth < Player.Instance.maxHealth / 2)
                    {
                        slot.slottedDie.value += 1;
                        DieAction.UpdateText(slot.slottedDie);
                    }
                    else
                    {
                        slot.slottedDie.value -= 1;
                        DieAction.UpdateText(slot.slottedDie);
                    }
                }
            }
        }

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slotTag == "Damage")
            {
                if (slot.slottedDie != null)
                {
                    if (Player.Instance.currentHealth < Player.Instance.maxHealth / 2)
                    {
                        slot.slottedDie.value -= 1;
                        DieAction.UpdateText(slot.slottedDie);
                    }
                    else
                    {
                        slot.slottedDie.value += 1;
                        DieAction.UpdateText(slot.slottedDie);
                    }
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
