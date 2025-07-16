namespace Diceonomicon

using UnityEngine;

public static class CombatManager : MonoBehaviour
{
    private DiceSlot[] PlayerColumn;
    private DiceSlot[] EnemyColumn;

    public void HandleActiveCombat() {
        Debug.Log("CombatManager.HandleActiveCombat");
    }

    private void HandleSlotEffects(_ActiveSlots) {
        Debug.Log("CombatManager.HandleSlotEffects");
    }

    private DiceSlot[][] SortActiveSlots(_ActiveColumn) {
        Debug.Log("CombatManager.SortActiveSlots");
    }

}