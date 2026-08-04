using System;
using System.Linq;

public class Lifedrink : Trait
{
    private int initialHealing;

    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "Heal by 1 whenever you use a Damage die";
        tablet.descText.text = description;

        BattleSceneManager.OnPlacementDone.AddListener(OnPlacementDone);
        BattleSceneManager.OnAcvitveCombatStart.AddListener(OnAcvitveCombatStart);
    }

    public void OnPlacementDone()
    {
        int healing = 0;

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slottedDie != null && slot.slottedDie.dieTags.Contains("Damage"))
            {
                healing++;
            }
        }

        healing -= initialHealing;
        initialHealing = healing;

        int newHealth = Player.Instance.currentHealth + healing;
        StartCoroutine(BattleSceneManager.Instance.AnimatePlayerHealthIncrease(newHealth, healing));
    }

    public void OnAcvitveCombatStart()
    {
        initialHealing = 0;
        int healing = 0;

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slottedDie != null && slot.slottedDie.dieTags.Contains("Damage"))
            {
                healing++;
            }
        }

        initialHealing = healing;
        int newHealth = Player.Instance.currentHealth + healing;
        StartCoroutine(BattleSceneManager.Instance.AnimatePlayerHealthIncrease(newHealth, healing));
    }

    public override void UnsubscribeFromEvents()
    {
        BattleSceneManager.OnPlacementDone.RemoveListener(OnPlacementDone);
        BattleSceneManager.OnAcvitveCombatStart.RemoveListener(OnAcvitveCombatStart);
    }
}