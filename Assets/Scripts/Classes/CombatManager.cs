
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
            if (slot.slottedDie != null)
            {
                slot.slottedDie.isDraggable = false;
            }

            if (slot.tag == "Buff" && slot.slottedDie != null && slot.isFilled == true)
            {
                slot.DoEffect();
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        Die[] unusedDice = GameObject.FindObjectsByType<Die>(FindObjectsSortMode.None);
        foreach (Die die in unusedDice)
        {
            if (die.isPlaced != true)
            {
                die.transform.position = new Vector3(10, 0, 0);
            }
        }

        sceneManager.combatBolt.SetActive(true);
        sceneManager.player.inColumnPhase = true;

        for (int column = 1; column <= 3; column++)
        {
            sceneManager.GetActiveColumn(column);


            foreach (GameObject banner in sceneManager.playerColumnBanner)
            {
                Image sprite = banner.GetComponent<Image>();
                sprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }

            foreach (GameObject opponentBanner in sceneManager.opponentColumnBanner)
            {
                Image sprite = opponentBanner.GetComponent<Image>();
                sprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }

            Image bannerSprite = sceneManager.playerColumnBanner[column - 1].GetComponent<Image>();
            bannerSprite.color = new Color(1f, 1f, 1f, 1f);
            Image opponentBannerSprite = sceneManager.opponentColumnBanner[column - 1].GetComponent<Image>();
            opponentBannerSprite.color = new Color(1f, 1f, 1f, 1f);



            playerSlots = SortActiveSlots(sceneManager.playerActiveColumn);
            enemySlots = SortActiveSlots(sceneManager.enemyActiveColumn);

            int delay = Mathf.Max(playerSlots.Count, enemySlots.Count);

            // for (int i = 0; i < playerSlots.Count; i++)
            // {
            //     var task = HandleSlotEffects(playerSlots[i]);
            //     await task;
            // }

            // for (int i = 0; i < enemySlots.Count; i++)
            // {
            //     var task = HandleSlotEffects(enemySlots[i]);
            //     await task;
            // }

            for (int i = 0; i < 2; i++)
            {
                var task1 = HandleSlotEffects(playerSlots[i]);
                await task1;
                var task2 = HandleSlotEffects(enemySlots[i]);
                await task2;                
            }

            currentColumn = column;


            playerSlots.Clear();
            enemySlots.Clear();
            await Task.Delay(TimeSpan.FromSeconds(delay + 2));
            sceneManager.CalculateDamage();
            await Task.Delay(TimeSpan.FromSeconds(1));
            sceneManager.ClearActiveColumn();
        }
        sceneManager.combatBolt.SetActive(false);

        foreach (GameObject banner in sceneManager.playerColumnBanner)
        {
            Image sprite = banner.GetComponent<Image>();
            sprite.color = new Color(1f, 1f, 1f, 1f);
        }

        foreach (GameObject opponentBanner in sceneManager.opponentColumnBanner)
        {
            Image sprite = opponentBanner.GetComponent<Image>();
            sprite.color = new Color(1f, 1f, 1f, 1f);
        }

    }

    private static async Task HandleSlotEffects(List<DiceSlotController> activeSlot)
    {
        foreach (DiceSlotController slot in activeSlot)
        {
            slot.DoEffect();
            await Task.Delay(TimeSpan.FromSeconds(1));
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
