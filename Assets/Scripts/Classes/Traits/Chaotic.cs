using System;
using UnityEngine;

public class Chaotic : Trait
{
    private DiceSlotData attackSlot;
    private DiceSlotData blockSlot;
    private DiceSlotData poisonSlot;
    private DiceSlotData mimicSlot;

    private DiceSlotData[] slots = new DiceSlotData[4];

    public void Start()
    {
        tablet = GetComponent<TabletController>();

        attackSlot = Resources.Load<DiceSlotData>($"Slots/AttackSlot");
        blockSlot = Resources.Load<DiceSlotData>($"Slots/ShieldSlot");
        poisonSlot = Resources.Load<DiceSlotData>($"Slots/PoisonSlot");
        mimicSlot = Resources.Load<DiceSlotData>($"Slots/MimicrySlot");

        slots = new DiceSlotData[] { attackSlot, blockSlot, poisonSlot, mimicSlot };

        description = "Randomizes the slots every round.";
        tablet.descText.text = description;

        BattleSceneManager.OnRoundStart.AddListener(OnRoundStart);
    }

    public void OnRoundStart()
    {
        SlotAction.ChangeSlotData(1, tablet, slots[(int)Math.Floor(UnityEngine.Random.Range(0f, 3.999f))]);
        SlotAction.ChangeSlotData(2, tablet, slots[(int)Math.Floor(UnityEngine.Random.Range(0f, 2.999f))]);
        SlotAction.ChangeSlotData(3, tablet, slots[(int)Math.Floor(UnityEngine.Random.Range(0f, 2.999f))]);
        SlotAction.ChangeSlotData(4, tablet, slots[(int)Math.Floor(UnityEngine.Random.Range(0f, 3.999f))]);
        SlotAction.ChangeSlotData(5, tablet, slots[(int)Math.Floor(UnityEngine.Random.Range(0f, 2.999f))]);
        SlotAction.ChangeSlotData(6, tablet, slots[(int)Math.Floor(UnityEngine.Random.Range(0f, 2.999f))]);
    }

    public override void UnsubscribeFromEvents()
    {
        BattleSceneManager.OnRoundStart.RemoveListener(OnRoundStart);
    }
}