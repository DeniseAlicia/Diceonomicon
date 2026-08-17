using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hunger : Trait
{
    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "Eat the lowest value die and heal by its value";
        tablet.descText.text = description;

        BattleSceneManager.OnAcvitveCombatStart.AddListener(OnAcvitveCombatStart);
    }

    public void OnAcvitveCombatStart()
    {
        int healing = 0;

        List<Die> placedDice = new List<Die>();

        foreach (Die die in Player.Instance.dice)
        {
            if (die.isPlaced
                && !die.statuses.Contains(Status.Inactive)
                && !die.statuses.Contains(Status.Protected)
                && !die.dieTags.Contains("Buff"))
            {
                placedDice.Add(die);
            }
        }

        if (placedDice.Count > 0)
        {
            Die lowestDie = placedDice.OrderBy(d => d.value).First();
            healing = lowestDie.value;

            lowestDie.parentSlot.isFilled = false;
            lowestDie.used = true;
            lowestDie.parentSlot.comboSlots.Clear();
            lowestDie.parentSlot.comboDisplay.ShowComboDisplay();
            lowestDie.parentSlot.slottedDie = null;

            Player.Instance.dice.Remove(lowestDie);
            GameObject dieObject = lowestDie.transform.gameObject;
            Destroy(dieObject);
        }

        Opponent.Instance.currentHealth += healing;
        Opponent.Instance.currentHealth = Mathf.Min(Opponent.Instance.currentHealth, Opponent.Instance.maxHealth);
        Opponent.Instance.healthText.text = Opponent.Instance.currentHealth.ToString();
    }

    public override void UnsubscribeFromEvents()
    {
        BattleSceneManager.OnAcvitveCombatStart.RemoveListener(OnAcvitveCombatStart);
    }
}