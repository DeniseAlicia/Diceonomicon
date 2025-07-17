namespace Diceonomicon
{
    using UnityEngine;

    public static class CombatManager
    {
        private DiceSlot[] PlayerSlots;
        private DiceSlot[] EnemySlots;

        public static void HandleActiveCombat(BattleScene _BattleScene)
        {
            Debug.Log("CombatManager.HandleActiveCombat");

            this.PlayerSlots = SortActiveSlots(_BattleScene.PlayerActiveColumn);
            this.EnemySlots = SortActiveSlots(_BattleScene.EnemyActiveColumn);

            for (int i = 0; i < PlayerSlots.length; i++)
            {
                HandleSlotEffects(PlayerSlots[i]);
                HandleSlotEffects(EnemySlots[i]);

            }
        }

        private static void HandleSlotEffects(DiceSlot[] _ActiveSlots)
        {
            Debug.Log("CombatManager.HandleSlotEffects");

        foreach (DiceSlot Slot in _ActiveSlots)
            {
                Slot.DoEffect();
            }
        }

        private static DiceSlot[][] SortActiveSlots(DiceSlot[] _ActiveColumn) {
            Debug.Log("CombatManager.SortActiveSlots");
        }

    }
}