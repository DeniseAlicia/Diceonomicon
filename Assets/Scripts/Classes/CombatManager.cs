
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public static class CombatManager
{
    private static List<List<DiceSlotController>> playerSlots;
    private static List<List<DiceSlotController>> enemySlots;
    public static int currentColumn { get; private set; }


    public static void HandleActiveCombat(BattleSceneManager sceneManager)
    {
        for (int column = 1; column <= 3; column++)
        {
            // Debug.Log("Column: " + column);
            sceneManager.GetActiveColumn(column);

            playerSlots = SortActiveSlots(sceneManager.playerActiveColumn);
            enemySlots = SortActiveSlots(sceneManager.enemyActiveColumn);

            currentColumn = column;

            for (int i = 0; i < playerSlots.Count; i++)
            {
                HandleSlotEffects(playerSlots[i]);
                HandleSlotEffects(enemySlots[i]);
            }

            playerSlots.Clear();
            enemySlots.Clear();
            sceneManager.CalculateDamage();
        }
    }

    private static void HandleSlotEffects(List<DiceSlotController> activeSlots)
    {
        // Debug.Log("CombatManager.HandleSlotEffects");

        foreach (DiceSlotController slot in activeSlots)
        {
            slot.DoEffect();
        }
    }

    private static List<List<DiceSlotController>> SortActiveSlots(List<DiceSlotController> activeColumn)
    {
        //Debug.Log("CombatManager.SortActiveSlots");
        List<DiceSlotController> priority1 = new List<DiceSlotController>();
        List<DiceSlotController> priority2 = new List<DiceSlotController>();
        List<DiceSlotController> priority3 = new List<DiceSlotController>();

        List<List<DiceSlotController>> sortedSlots = new List<List<DiceSlotController>>();

        foreach (DiceSlotController slot in activeColumn)
        {

            if (slot.isFilled && !slot.isHandled)
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

        sortedSlots.Add(priority1);
        sortedSlots.Add(priority2);
        sortedSlots.Add(priority3);

        return sortedSlots;

    }
}
