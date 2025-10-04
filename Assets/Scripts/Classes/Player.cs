using UnityEngine;
using System.Collections.Generic;

public class Player : Entity
{
    public List<string> ImplingRoster;
    public List<TabletData> ActiveImplings;
    public int MaxImplings;
    public List<Die> dice;

    public int level;
    public string area;

    public List<TabletData> SetImplingRoster()
    {
        ImplingRoster.Add("Cubie");
        ImplingRoster.Add("Stabo");
        ImplingRoster.Add("Spooding");

        foreach (var impName in ImplingRoster)
        {
            string dataName = impName + "Data";

            TabletData data = Resources.Load<TabletData>($"Implings/{dataName}");
            if (data != null)
            {
                ActiveImplings.Add(data);
            }
        }
        return ActiveImplings;
    }

    public void CreateDiceDeck()
    {
        foreach (TabletData impling in ActiveImplings)
        {
            for (int i = 0; i < impling.startingDice.Length; i++)
            {
                diceDeck.Add(impling.startingDice[i]);
            }
        }
    }

    public override void RollDice()
    {
        dice = new List<Die>();
        Vector3 startPosition = new Vector3(0f, 5f, -5f);
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

        // Debug.Log("Slots: " + string.Join(", ", dice));

        foreach (Die dieInstance in dice)
        {
            dieInstance.Roll(0.02f);
        }
    }
}
