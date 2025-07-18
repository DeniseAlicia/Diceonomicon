namespace Diceonomicon
{
    using UnityEngine;
    using System.Collections.Generic;

    public static class CombatManager
    {
        private static List<List<DiceSlot>> playerSlots;
        private static List<List<DiceSlot>> enemySlots;

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

        private static void HandleSlotEffects(List<DiceSlot> _activeSlots)
        {
            Debug.Log("CombatManager.HandleSlotEffects");

            foreach (DiceSlot slot in _activeSlots)
            {
                slot.DoEffect();
            }
        }

        private static List<List<DiceSlot>> SortActiveSlots(List<DiceSlot> _activeColumn)
        {
            Debug.Log("CombatManager.SortActiveSlots");
            List<DiceSlot> priority1 = new List<DiceSlot>();
            List<DiceSlot> priority2 = new List<DiceSlot>();
            List<DiceSlot> priority3 = new List<DiceSlot>();
            foreach (DiceSlot slot in _activeColumn)
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

            List<List<DiceSlot>> sortedSlots = new List<List<DiceSlot>>();
            sortedSlots.Add(priority1);
            sortedSlots.Add(priority2);
            sortedSlots.Add(priority3);
            return sortedSlots;

        }
    }
}