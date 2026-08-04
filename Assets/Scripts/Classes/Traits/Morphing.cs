using UnityEngine;

public class Morphing : Trait
{
    private bool aggressive;

    private DiceSlotData attackSlot;
    private DiceSlotData blockSlot;

    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "Swaps Attack and Shield slot positions every round.";
        tablet.descText.text = description;

        attackSlot = Resources.Load<DiceSlotData>($"Slots/AttackSlot");
        blockSlot = Resources.Load<DiceSlotData>($"Slots/ShieldSlot");

        BattleSceneManager.OnRoundStart.AddListener(OnRoundStart);
    }

    public void OnRoundStart()
    {
        if (aggressive)
        {
            SlotAction.ChangeSlotData(1, tablet, attackSlot);
            SlotAction.ChangeSlotData(2, tablet, attackSlot);
            SlotAction.ChangeSlotData(4, tablet, blockSlot);
            SlotAction.ChangeSlotData(5, tablet, blockSlot);
            SlotAction.ChangeSlotData(6, tablet, attackSlot);
            SlotAction.ChangeSlotData(8, tablet, blockSlot);
            SlotAction.ChangeSlotData(9, tablet, attackSlot);
            aggressive = false;
        }
        else
        {
            SlotAction.ChangeSlotData(1, tablet, blockSlot);
            SlotAction.ChangeSlotData(2, tablet, blockSlot);
            SlotAction.ChangeSlotData(4, tablet, attackSlot);
            SlotAction.ChangeSlotData(5, tablet, attackSlot);
            SlotAction.ChangeSlotData(6, tablet, blockSlot);
            SlotAction.ChangeSlotData(8, tablet, attackSlot);
            SlotAction.ChangeSlotData(9, tablet, blockSlot);
            aggressive = true;
        }
    }

    public override void UnsubscribeFromEvents()
    {
        BattleSceneManager.OnRoundStart.RemoveListener(OnRoundStart);
    }
}