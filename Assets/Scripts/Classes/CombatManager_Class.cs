namespace Diceonomicon
{
using UnityEngine;

public static class CombatManager : MonoBehaviour
{
    private DiceSlot[] PlayerColumn;
    private DiceSlot[] EnemyColumn;

    public void HandleActiveCombat() {
        Debug.Log("CombatManager.HandleActiveCombat");
    }

        private static void HandleSlotEffects(DiceSlot[] _ActiveSlots)
        {
        Debug.Log("CombatManager.HandleSlotEffects");
    }

        private static DiceSlot[][] SortActiveSlots(DiceSlot[] _ActiveColumn) {
        Debug.Log("CombatManager.SortActiveSlots");
    }

    }
}