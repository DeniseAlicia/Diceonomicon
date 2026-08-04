using System;
using System.Linq;

public class Hex : Trait
{
    private int initialDamage;

    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "Deal 1 damage whenever you use a Spell die";
        tablet.descText.text = description;

        BattleSceneManager.OnPlacementDone.AddListener(OnPlacementDone);
        BattleSceneManager.OnAcvitveCombatStart.AddListener(OnAcvitveCombatStart);
    }

    public void OnPlacementDone()
    {
        int damage = 0;

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slottedDie != null && slot.slottedDie.dieTags.Contains("Spell"))
            {
                damage++;
            }
        }

        damage -= initialDamage;
        initialDamage = damage;

        int newHealth = Opponent.Instance.currentHealth - damage;
        StartCoroutine(BattleSceneManager.Instance.AnimateOpponentHealthDecrease(newHealth, damage));
    }

    public void OnAcvitveCombatStart()
    {
        int damage = 0;

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slottedDie != null && slot.slottedDie.dieTags.Contains("Spell"))
            {
                damage++;
            }
        }

        initialDamage = damage;
        int newHealth = Opponent.Instance.currentHealth - damage;
        StartCoroutine(BattleSceneManager.Instance.AnimateOpponentHealthDecrease(newHealth, damage));
    }

        public override void UnsubscribeFromEvents()
    {
        BattleSceneManager.OnPlacementDone.RemoveListener(OnPlacementDone);
        BattleSceneManager.OnAcvitveCombatStart.RemoveListener(OnAcvitveCombatStart);
    }
}