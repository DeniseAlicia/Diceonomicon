using System.Linq;
using UnityEngine;

public class Adored : Trait
{
    private TabletController[] tablets;
    public bool couple;
    private int initialDamage;

    public void Start()
    {
        tablet = GetComponent<TabletController>();
        tablets = FindObjectsByType<TabletController>(FindObjectsSortMode.None);

        if (tablet.nameText.text == "Xiont")
        {
            description = "Deal 1 damage whenever you use a Spell die. Effect is doubled if together with Jaunt";
            tablet.descText.text = description;
        }
        else if (tablet.nameText.text == "Jaunt")
        {
            description = "Draw 1 extra die each round. Effect is doubled if together with Xiont";
            tablet.descText.text = description;
        }

        foreach (TabletController otherTablet in tablets)
        {
            string name = otherTablet.nameText.text;
            if (tablet.nameText.text == "Jaunt" && name == "Xiont" || tablet.nameText.text == "Xiont" && name == "Jaunt")
            {
                couple = true;
            }
        }

        if (tablet.nameText.text == "Jaunt" && Player.Instance != null)
        {
            Player.Instance.maxDrawSize += 1;
            if (couple)
            {
                Player.Instance.maxDrawSize += 1;
            }
        }

        BattleSceneManager.OnPlacementDone.AddListener(OnPlacementDone);
        BattleSceneManager.OnAcvitveCombatStart.AddListener(OnAcvitveCombatStart);
    }

    public void OnPlacementDone()
    {
        int damage = 0;
        if (tablet.nameText.text == "Xiont")
        {
            foreach (DiceSlotController slot in tablet.tabletSlots)
            {
                if (slot.slottedDie != null && slot.slottedDie.dieTags.Contains("Spell"))
                {
                    damage++;
                    if (couple)
                    {
                        damage++;
                    }
                }
            }

            damage -= initialDamage;
            initialDamage = damage;

            int newHealth = Opponent.Instance.currentHealth - damage;
            StartCoroutine(BattleSceneManager.Instance.AnimateOpponentHealthDecrease(newHealth, damage));
        }
    }

    public void OnAcvitveCombatStart()
    {
        int damage = 0;
        if (tablet.nameText.text == "Xiont")
        {
            foreach (DiceSlotController slot in tablet.tabletSlots)
            {
                if (slot.slottedDie != null && slot.slottedDie.dieTags.Contains("Spell"))
                {
                    damage++;
                    if (couple)
                    {
                        damage++;
                    }
                }
            }

            damage -= initialDamage;
            initialDamage = damage;

            int newHealth = Opponent.Instance.currentHealth - damage;
            StartCoroutine(BattleSceneManager.Instance.AnimateOpponentHealthDecrease(newHealth, damage));
        }
    }

    public override void UnsubscribeFromEvents()
    {
        BattleSceneManager.OnPlacementDone.RemoveListener(OnPlacementDone);
        BattleSceneManager.OnAcvitveCombatStart.RemoveListener(OnAcvitveCombatStart);
    }
}