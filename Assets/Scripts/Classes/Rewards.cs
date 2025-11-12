using UnityEngine;
using System.Collections.Generic;

public class Rewards
{
    private static Dictionary<(string type, int quality, string area), List<List<string>>> rewardTable
        = new Dictionary<(string, int, string), List<List<string>>>()
        {
         { ("Dice", 0, "None"), new List<List<string>>
            {
                new List<string>{ "BasicDamageDie" },
            }
        },
        { ("Dice", 1, "Red"), new List<List<string>>
            {
                new List<string>{ "BasicDamageDie"},
                new List<string>{ "ExplodingDamageDie" },
                new List<string>{ "ScorchedDamageDie"},
                new List<string>{ "BasicNeutralDie" },
                new List<string>{ "EncoreDamageDie" },
                new List<string>{ "ChainDamageDie"}
            }
        },
    };

    public static void GetRewards(string type, int quality, string area, int amount)
    {
        if (!rewardTable.TryGetValue((type, quality, area), out var rewardList))
        {
            rewardList = rewardTable[("None", 0, "None")];
        }
        int randomIndex = UnityEngine.Random.Range(0, rewardList.Count);
        List<string> rewards = rewardList[randomIndex];

        GetRandomSelection(type, rewards, amount);

    }

    private static void GetRandomSelection(string type, List<string> rewards, int amount)
    {
        switch (type)
        {
            case "Dice":
                List<DiceData> options = new();
                foreach (string reward in rewards)
                {
                    string dataName = reward + "_Data";

                    DiceData dieData = Resources.Load<DiceData>($"Dice/{dataName}");
                    if (dieData != null)
                    {
                        options.Add(dieData);
                    }
                    else
                    {
                        Debug.LogWarning($"Could not load Data for {reward} (looked for {dataName})");
                    }
                }
                break;
            case "Impling":
                // code block
                break;
            default:
                // code block
                break;
        }
    }
}