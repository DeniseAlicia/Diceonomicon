using UnityEngine;

public class DoubleTrouble : Trait
{
    private bool isFilled;
    private bool isInEffect;

    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "Whena every slot is filled, double all the dice values";
        tablet.descText.text = description;

        BattleSceneManager.OnRoundStart.AddListener(OnRoundStart);
        BattleSceneManager.OnPlacementDone.AddListener(OnPlacementDone);
        BattleSceneManager.OnAcvitveCombatStart.AddListener(OnAcvitveCombatStart);
    }

    public void Update()
    {
        if (isFilled && !isInEffect)
        {
            foreach (DiceSlotController slot in tablet.tabletSlots)
            {
                slot.slottedDie.value = slot.slottedDie.value + slot.slottedDie.value;
                DieAction.UpdateText(slot.slottedDie);
            }

            isInEffect = true;
        }

        if (!isFilled)
        {
            isInEffect = false;
        }
    }

    public void OnRoundStart()
    {
        isFilled = false;
        isInEffect = false;
    }

    public void OnPlacementDone()
    {
        int slotsFilled = 0;

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slottedDie != null)
            {
                slotsFilled += 1;
            }
        }

        if (!isInEffect && slotsFilled == tablet.tabletSlots.Count)
        {
            isFilled = true;
            isInEffect = true;
            DoubleDiceValues();
        }
        else
        {
            isFilled = false;
        }
    }

    public void OnAcvitveCombatStart()
    {
        int slotsFilled = 0;

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slottedDie != null)
            {
                slotsFilled += 1;
            }
        }

        if (!isInEffect && slotsFilled == tablet.tabletSlots.Count)
        {
            isFilled = true;
            isInEffect = true;
            DoubleDiceValues();
        }
        else
        {
            isFilled = false;
        }
    }


    public void DoubleDiceValues()
    {
        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            slot.slottedDie.value = slot.slottedDie.value + slot.slottedDie.value;
            DieAction.UpdateText(slot.slottedDie);
        }
    }


    public override void UnsubscribeFromEvents()
    {
        BattleSceneManager.OnRoundStart.RemoveListener(OnRoundStart);
        BattleSceneManager.OnPlacementDone.RemoveListener(OnPlacementDone);
        BattleSceneManager.OnAcvitveCombatStart.RemoveListener(OnAcvitveCombatStart);
    }
}
