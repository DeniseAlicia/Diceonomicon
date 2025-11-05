using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
        List<string> implings = new List<string> { "MrMimic", "Stabo", "Spike", "Hie", "Cubie", "Beempling", "Haunt", "Spooding" };

        System.Random rng = new System.Random();
        int n = implings.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (implings[k], implings[n]) = (implings[n], implings[k]);
        }

        for (int i = 0; i < 3; i++)
        {
            ImplingRoster.Add(implings[i]);
        }

        // Check if it's the tutorial
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            ImplingRoster.Clear();
            ImplingRoster.Add("Tutorial");
        }

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
        Vector3 startPosition = new Vector3(-1f, 5f, -5f);
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
            dieInstance.Roll(0.05f);
        }
    }
}
