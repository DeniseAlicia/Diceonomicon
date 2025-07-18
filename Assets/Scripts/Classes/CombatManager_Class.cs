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

            List<DiceSlot> prio1 = new List<DiceSlot>();
            List<DiceSlot> prio2 = new List<DiceSlot>();
            List<DiceSlot> prio3 = new List<DiceSlot>();
            List<DiceSlot> prio4 = new List<DiceSlot>();

            foreach (DiceSlot slot in _activeColumn)
            {
                if (slot.filled)
                {
                    slot.DetectLinks();
                    switch (slot.type)
                    {
                        case "Buff":
                            prio1.Add(slot);
                            break;
                        case "Spell":
                            prio2.Add(slot);
                            break;
                        case "Block":
                            prio3.Add(slot);
                            break;
                        case "Damage":
                            prio4.Add(slot);
                            break;
                    }
                }
            }
            List<List<DiceSlot>> sortedSlots = new List<List<DiceSlot>>();
            sortedSlots.Add(prio1);
            sortedSlots.Add(prio2);
            sortedSlots.Add(prio3);
            sortedSlots.Add(prio4);
            return sortedSlots;

        }
    }
}