
using UnityEngine;
using System.Collections.Generic;

public class Opponent : Entity
{
    public TabletData[] army;
    public GameObject ai;
    public List<Die> dice;

    public void SetEnemyRoster()
    {
        foreach (TabletData demon in army)
        {
            for (int i = 0; i < demon.startingDice.Length; i++)
            {
                diceDeck.Add(demon.startingDice[i]);
            }
        }
    }

    public override void RollDice()
    {
        List<Die> dice = new List<Die>();
        Vector3 startPosition = new Vector3(0f, 15f, -15f);
        float distance = 0.5f;

        for (int i = 0; i < drawnDice.Count; i++)
        {
            DiceData dieData = drawnDice[i];

            float overflow = Mathf.Floor(i / 3f);
            float spacing = (i - overflow * 3) * distance;

            Vector3 diePos = startPosition;
            diePos.x += spacing;
            diePos.z += overflow * distance;

            // Instantiate prefab at the calculated position
            GameObject dieObject = Instantiate(diePrefab, diePos, Quaternion.identity);

            // Set data on the die script
            Die die = dieObject.GetComponent<Die>();
            die.SetData(dieData);

            // Add dice to die Class list
            dice.Add(die);
        }

        foreach (Die dieInstance in dice)
        {
            dieInstance.Roll();
        }
    }
}
