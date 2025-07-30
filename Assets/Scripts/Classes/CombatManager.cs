
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public static class CombatManager
{
    private static List<List<DiceSlotController>> playerSlots;
    private static List<List<DiceSlotController>> enemySlots;
    public static int currentColumn { get; private set; }

    public static async void HandleActiveCombat(BattleSceneManager sceneManager)
    {
        DiceSlotController[] slots = GameObject.FindObjectsByType<DiceSlotController>(FindObjectsSortMode.None);
        foreach (DiceSlotController slot in slots)
        {
            if (slot.tag == "Buff" && slot.slottedDie != null && slot.isFilled == true)
            {
                slot.DoEffect();
            }
        }

        sceneManager.combatBolt.SetActive(true);
        sceneManager.player.inColumnPhase = true;

        for (int column = 1; column <= 3; column++)
        {

            // sceneManager.player.alpha = Mathf.MoveTowards(0.1f, 0.9f, 0.02f * Time.deltaTime);
            // sceneManager.opponent.alpha = Mathf.MoveTowards(0.1f, 0.9f, 0.02f * Time.deltaTime);
            sceneManager.GetActiveColumn(column);

            playerSlots = SortActiveSlots(sceneManager.playerActiveColumn);
            enemySlots = SortActiveSlots(sceneManager.enemyActiveColumn);

            int delay = Mathf.Max(playerSlots.Count, enemySlots.Count) + 2;

            for (int i = 0; i < playerSlots.Count; i++)
            {
                HandleSlotEffects(playerSlots[i]);
            }

            for (int i = 0; i < enemySlots.Count; i++)
            {
                HandleSlotEffects(enemySlots[i]);
            }

            currentColumn = column;


            playerSlots.Clear();
            enemySlots.Clear();
            await Task.Delay(TimeSpan.FromSeconds(delay));
            sceneManager.CalculateDamage();
            sceneManager.ClearActiveColumn();
        }
        sceneManager.combatBolt.SetActive(false);
    }

    private static async void HandleSlotEffects(List<DiceSlotController> activeSlot)
    {
        foreach (DiceSlotController slot in activeSlot)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
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

        //sortedSlots.Add(priority1);
        sortedSlots.Add(priority2);
        sortedSlots.Add(priority3);

        return sortedSlots;

    }
}
