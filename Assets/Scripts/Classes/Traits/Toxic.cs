using UnityEngine;
using System;
using System.Linq;

public class Toxic : Trait
{
    private TabletController tablet;

    private int initialDamage;

    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "Deal 1 damage whenever you poison a die";
        tablet.descText.text = description;

        BattleSceneManager.OnSlotTriggered += OnSlotTriggered;
    }


    public override void OnSceneStart() { }

    public override void OnRoundStart() { }

    public override void OnPlacementDone() { }

    public override void OnAcvitveCombatStart() { }

    public override void OnAcvitveCombatEnd() { }

    public void OnSlotTriggered(DiceSlotController slot)
    {
        if (slot.slottedDie != null && tablet.tabletSlots.Contains(slot) && slot.tag == "Spell")
        {
            int damage = slot.slotData.affectedDice;

            int newHealth = Player.Instance.currentHealth - damage;
            StartCoroutine(BattleSceneManager.Instance.AnimatePlayerHealthDecrease(newHealth, damage));
        }
    }
}
