using UnityEngine;

public class Main
{
    public static readonly Color[] colors = new Color[]
        {
        Color.red,
        new Color(a: 1.0f, r: 0.0f, g: 0.5f, b: 1.0f),
        Color.green,
        Color.purple,
        Color.white,
        Color.yellow
        };

    public static readonly string[] diceTags = new string[]
    {
        "Damage", "Block", "Buff", "Spell", "Neutral", "Debuff"
    };

    public static readonly string[] areas = { "Red", "Blue", "Green", "Purple", "White", "Yellow" };

    public static void ChangeSlotData(int slotNumber, TabletController tablet, DiceSlotData newSlot)
    {
        int index = slotNumber - 1;
        DiceSlotController slotController = tablet.tabletSlots[index];
        slotController.SetData(newSlot);
    }
}
