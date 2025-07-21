
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public static class CombatManager
{
    private static List<List<DiceSlotData>> playerSlots;
    private static List<List<DiceSlotData>> enemySlots;

    public static void HandleActiveCombat(BattleScene _battleScene)
    {
        Debug.Log("CombatManager.HandleActiveCombat");

        playerSlots = SortActiveSlots(_battleScene.playerActiveColumn);
        enemySlots = SortActiveSlots(_battleScene.enemyActiveColumn);

        for (int i = 0; i < playerSlots.Count; i++)
        {
            HandleSlotEffects(playerSlots[i]);
            HandleSlotEffects(enemySlots[i]);

        }
    }

    private static void HandleSlotEffects(List<DiceSlotData> _activeSlots)
    {
        Debug.Log("CombatManager.HandleSlotEffects");

        foreach (DiceSlotData slot in _activeSlots)
        {
            slot.DoEffect();
        }
    }

    private static List<List<DiceSlotData>> SortActiveSlots(List<DiceSlotData> _activeColumn)
    {
        Debug.Log("CombatManager.SortActiveSlots");
        List<DiceSlotData> priority1 = new List<DiceSlotData>();
        List<DiceSlotData> priority2 = new List<DiceSlotData>();
        List<DiceSlotData> priority3 = new List<DiceSlotData>();
        foreach (DiceSlotData slot in _activeColumn)
        {
            if (slot.filled)
            {
                switch (slot.priority)
                {
                    case 1:
                        priority1.Add(slot);
                        break;
                    case 2:
                        priority2.Add(slot);
                        break;
                    case 3:
                        priority3.Add(slot);
                        break;
                }
            }
        }

        List<List<DiceSlotData>> sortedSlots = new List<List<DiceSlotData>>();
        sortedSlots.Add(priority1);
        sortedSlots.Add(priority2);
        sortedSlots.Add(priority3);
        return sortedSlots;

    }
}
