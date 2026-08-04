public class Toxic : Trait
{
    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "Deal 1 damage whenever you poison a die";
        tablet.descText.text = description;

        BattleSceneManager.OnSlotTriggered += OnSlotTriggered;
    }

    public void OnSlotTriggered(DiceSlotController slot)
    {
        if (slot.slottedDie != null && tablet.tabletSlots.Contains(slot) && slot.tag == "Spell")
        {
            int damage = slot.slotData.affectedDice;

            int newHealth = Player.Instance.currentHealth - damage;
            StartCoroutine(BattleSceneManager.Instance.AnimatePlayerHealthDecrease(newHealth, damage));
        }
    }

    public override void UnsubscribeFromEvents()
    {
        BattleSceneManager.OnSlotTriggered -= OnSlotTriggered;
    }
}