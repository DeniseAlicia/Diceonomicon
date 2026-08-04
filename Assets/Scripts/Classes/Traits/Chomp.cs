using System.Collections.Generic;
using UnityEngine;

public class Chomp : Trait
{
    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "Eat a random die and heal by its value";
        tablet.descText.text = description;

        BattleSceneManager.OnAcvitveCombatStart.AddListener(OnAcvitveCombatStart);
    }

    public void OnAcvitveCombatStart()
    {
        List<Die> placedDice = new List<Die>();

        foreach (Die die in Player.Instance.dice)
        {
            if (die.isPlaced
                && !die.statuses.Contains(Status.Inactive)
                && !die.statuses.Contains(Status.Protected))
            {
                placedDice.Add(die);
            }
        }

        if (placedDice.Count > 0)
        {
            int randomIndex = Random.Range(0, placedDice.Count);
            Die randomDie = placedDice[randomIndex];

            randomDie.parentSlot.isFilled = false;
            randomDie.parentSlot.slottedDie = null;

            Player.Instance.dice.Remove(randomDie);
            GameObject dieObject = randomDie.transform.gameObject;
            Destroy(dieObject);
        }
    }

    public override void UnsubscribeFromEvents()
    {
        BattleSceneManager.OnAcvitveCombatStart.RemoveListener(OnAcvitveCombatStart);
    }
}