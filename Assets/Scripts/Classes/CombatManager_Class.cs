namespace Diceonomicon
{
    using UnityEngine;

    public static class CombatManager
    {
        private static DiceSlot[][] playerSlots;
        private static DiceSlot[][] enemySlots;

        public static void HandleActiveCombat(BattleScene _battleScene)
        {
            Debug.Log("CombatManager.HandleActiveCombat");

            CombatManager.playerSlots = SortActiveSlots(_battleScene.playerActiveColumn);
            CombatManager.enemySlots = SortActiveSlots(_battleScene.enemyActiveColumn);

            for (int i = 0; i < playerSlots.Length; i++)
            {
                HandleSlotEffects(playerSlots[i]);
                HandleSlotEffects(enemySlots[i]);

            }
        }

        private static void HandleSlotEffects(DiceSlot[] _activeSlots)
        {
            Debug.Log("CombatManager.HandleSlotEffects");

        foreach (DiceSlot slot in _activeSlots)
            {
                slot.DoEffect();
            }
        }

        private static DiceSlot[][] SortActiveSlots(DiceSlot[] _activeColumn)
        {
            Debug.Log("CombatManager.SortActiveSlots");
        }

    }
}