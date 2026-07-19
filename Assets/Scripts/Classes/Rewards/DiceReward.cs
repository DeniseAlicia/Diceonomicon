using UnityEngine;
using System.Collections.Generic;
using System;

public class DiceReward
{
    public static List<string> options;
    public static int quality;
    private static readonly string[] areas = { "Red", "Blue", "Green", "Purple", "White", "Yellow" };

    private static readonly Dictionary<(string type, string area),
        (List<List<(string reward, float prob)>> lists, List<float> qualityDistribution)> rewardTable
            = new()
        {{
        ("Dice", "None"),(
        new List<List<(string reward, float prob)>>
        {
            new ()
            {
                ("BasicDamageDie",      2f),
                ("BasicBlockDie",       2f),
                ("BasicSpellDie",       1f),
                ("BasicBuffDie",        1f),
                ("BasicDebuffDie",      0.5f),
                ("BasicNeutralDie",     0.5f),
            },
            new ()
            {
                ("BasicNeutralDie",    0.2f),
                ("BasicNeutralDie",    0.2f),
                ("BasicNeutralDie",    0.2f),
                ("BasicNeutralDie",    0.2f),
                ("BasicNeutralDie",    0.2f)
            }
        },
        qualityDistribution: new List<float> { 100f, 0f, 0f, 0f, 0f })
        },{
        ("Dice", "Red"),(
        new List<List<(string reward, float prob)>>
        {
            new ()
            {
                ("BasicDamageDie",      3f),
                ("BasicDamageDie",      3f),
                ("BasicNeutralDie",     1f),
                ("BasicNeutralDie",     0.1f),
            },
            new ()
            {
                ("ScorchedDamageDie",   1f),
                ("CrackedDamageDie",    1f),
                ("ChainDamageDie",      1f),
                ("BasicNeutralDie",     0.1f),
            },
            new ()
            {
                ("ScorchedDamageDie",   1f),
                ("CrackedDamageDie",    1f),
                ("ChainDamageDie",      1f),
                ("BasicNeutralDie",     0.1f),
            }
        },
        qualityDistribution: new List<float> { 100f, 0f, 0f, 0f, 0f })
        },{
        ("Dice", "Blue"),(
        new List<List<(string reward, float prob)>>
        {
            new ()
            {
                ("BasicBlockDie",   3f),
                ("BasicBlockDie",   3f),
                ("BasicNeutralDie", 1f),
                ("BasicNeutralDie", 0.1f),
            },
            new ()
            {
                ("ScorchedBlockDie",    1f),
                ("CrackedBlockDie",     1f),
                ("ChainBlockDie",       1f),
                ("BasicNeutralDie",     0.1f),
            },
            new ()
            {
                ("ScorchedBlockDie",    1f),
                ("CrackedBlockDie",     1f),
                ("ChainBlockDie",       1f),
                ("BasicNeutralDie",     0.1f),

            }
        },
        qualityDistribution: new List<float> { 100f, 0f, 0f, 0f, 0f })
        },{
        ("Dice", "Green"),(
        new List<List<(string reward, float prob)>>
        {
            new ()
            {
                ("BasicBuffDie",        3f),
                ("BasicBuffDie",        3f),
                ("BasicNeutralDie",     1f),
                ("BasicNeutralDie",     0.1f),
            },
            new ()
            {
                ("BasicBuffDie",        1f),
                ("BasicBuffDie",        1f),
                ("BasicNeutralDie",     1f),
                ("BasicNeutralDie",     0.1f),
            },
            new ()
            {
                ("BasicBuffDie",        1f),
                ("BasicBuffDie",        1f),
                ("BasicNeutralDie",     1f),
                ("BasicNeutralDie",     0.1f),
            }
        },
        qualityDistribution: new List<float> { 100f, 0f, 0f, 0f, 0f })
        },{
        ("Dice", "Purple"),(
        new List<List<(string reward, float prob)>>
        {
             new ()
            {
                ("BasicSpellDie",       3f),
                ("BasicSpellDie",       3f),
                ("BasicNeutralDie",     1f),
                ("BasicNeutralDie",     0.1f),
            },
            new ()
            {
                ("ScorchedSpellDie",    1f),
                ("CrackedSpellDie",     1f),
                ("ChainSpellDie",       1f),
                ("BasicNeutralDie",     0.1f),
            },
            new ()
            {
                ("ScorchedSpellDie",    1f),
                ("CrackedSpellDie",     1f),
                ("ChainSpellDie",       0.5f),
                ("BasicNeutralDie",     0.1f),
            }
        },
        qualityDistribution: new List<float> { 100f, 0f, 0f, 0f, 0f })
        },{
        ("Dice", "White"),(
        new List<List<(string reward, float prob)>>
        {
                new ()
            {
                ("BasicNeutralDie",     3f),
                ("BasicNeutralDie",     3f),
                ("BasicNeutralDie",     3f),
                ("BasicSpellDie",       1f),
                ("BasicDamageDie",      1f),
                ("BasicBlockDie",       1f),
                ("BasicBuffDie",        0.1f),
                ("BasicDebuffDie",      0.1f),
            },
            new ()
            {
                ("ScorchedNeutralDie",    1f),
                ("CrackedNeutralDie",     1f),
                ("BasicNeutralDie",       0.1f),
            },
            new ()
            {
                ("ScorchedNeutralDie",    1f),
                ("CrackedNeutralDie",     1f),
                ("BasicNeutralDie",       0.1f),
            }
        },
        qualityDistribution: new List<float> { 100f, 0f, 0f, 0f, 0f })
        },{
        ("Dice", "Yellow"),(
        new List<List<(string reward, float prob)>>
        {
                new ()
            {
                ("BasicDebuffDie",      3f),
                ("BasicDebuffDie",      3f),
                ("BasicNeutralDie",     1f),
                ("BasicNeutralDie",     0.1f),
            },
            new ()
            {
                ("ScorchedDebuffDie",   1f),
                ("CrackedDebuffDie",    1f),
                ("BasicDebuffDie",     0.5f),
                ("BasicNeutralDie",     0.1f),
            },
            new ()
            {
                ("ScorchedDebuffDie",   1f),
                ("CrackedDebuffDie",    1f),
                ("BasicDebuffDie",     0.5f),
                ("BasicNeutralDie",     0.1f),
            }
        },
        qualityDistribution: new List<float> { 100f, 0f, 0f, 0f, 0f })
        }
        };

    public static void GetRewards(string type, int quality, string area, int amount)
    {
        if (!rewardTable.TryGetValue((type, area), out var entry))
        {
            entry = rewardTable[("Dice", "None")];
        }

        entry.qualityDistribution = new List<float> { 0f, 0f, 0f, 0f, 0f };
        SetQuality(entry.qualityDistribution, quality + 1);

        var rewardLists = entry.lists;
        var qualityWeights = entry.qualityDistribution;

        int tier = GetWeightedIndex(qualityWeights);
        tier = Mathf.Clamp(tier, 0, rewardLists.Count - 1);

        // First Choice
        var pool = new List<(string reward, float prob)>(rewardLists[tier]);

        List<(string reward, float prob)> selected = new();

        amount = Mathf.Clamp(amount, 0, pool.Count);

        for (int i = 0; i < amount - 2; i++)
        {
            var chosen = GetWeightedRandom(pool);
            selected.Add(chosen);
            pool.Remove(chosen);
        }

        // Second Choice
        string neighborArea = GetNeighborArea(area);

        if (rewardTable.TryGetValue((type, neighborArea), out var neighborEntry))
        {
            var neighborList = neighborEntry.lists[tier];
            var extendedPool = new List<(string reward, float prob)>(neighborList);
            var chosen = GetWeightedRandom(extendedPool);
            selected.Add(chosen);
            extendedPool.Remove(chosen);
        }

        // Third Choice
        string randomArea = GetRandomArea();

        if (rewardTable.TryGetValue((type, randomArea), out var randomEntry))
        {
            var randomList = randomEntry.lists[tier];
            var fullPool = new List<(string reward, float prob)>(randomList);
            var chosen = GetWeightedRandom(fullPool);
            selected.Add(chosen);
            fullPool.Remove(chosen);
        }

        GetRandomSelection(type, selected);
    }

    public static void SetQuality(List<float> qualityDistributions, int quality)
    {
        switch (quality)
        {
            case 0: // Common
                qualityDistributions[0] = 80f;
                if (qualityDistributions.Count > 1) qualityDistributions[1] = 20f;
                break;
            case 1: // Uncommon
                if (qualityDistributions.Count > 1) qualityDistributions[1] = 90f;
                if (qualityDistributions.Count > 2) qualityDistributions[2] = 10f;
                break;
            case 2: // Rare
                if (qualityDistributions.Count > 1) qualityDistributions[1] = 10f;
                if (qualityDistributions.Count > 2) qualityDistributions[2] = 80f;
                if (qualityDistributions.Count > 3) qualityDistributions[3] = 10f;
                break;
            case 3: // Very Rare
                if (qualityDistributions.Count > 2) qualityDistributions[2] = 30f;
                if (qualityDistributions.Count > 3) qualityDistributions[3] = 70f;
                break;
            case 4: // Legendary
                if (qualityDistributions.Count > 3) qualityDistributions[3] = 10f;
                if (qualityDistributions.Count > 4) qualityDistributions[4] = 80f;
                break;
            default:
                qualityDistributions[0] = 100f;
                break;
        }
    }

    public static void GetRandomSelection(string type, List<(string reward, float prob)> rewards)
    {
        options = new();

        switch (type)
        {
            case "Dice":
                foreach (var (reward, prob) in rewards)
                    options.Add(reward);
                break;
            case "Impling":
                // TODO
                break;
            default:
                Debug.LogWarning($"Reward type '{type}' not recognized.");
                break;
        }
    }

    public static int GetWeightedIndex(List<float> weights)
    {
        float totalWeight = 0f;
        foreach (var weight in weights)
            totalWeight += weight;

        float roll = UnityEngine.Random.value;
        float cumulative = 0f;

        for (int i = 0; i < weights.Count; i++)
        {
            float normalized = weights[i] / totalWeight;
            cumulative += normalized;

            if (roll <= cumulative)
                return i;
        }

        return weights.Count - 1;
    }

    public static (string reward, float prob) GetWeightedRandom(List<(string reward, float prob)> weights)
    {
        float totalWeight = 0f;
        foreach (var (_, prob) in weights)
            totalWeight += prob;

        float roll = UnityEngine.Random.value;

        float cumulative = 0f;

        foreach (var item in weights)
        {
            float normalized = item.prob / totalWeight;
            cumulative += normalized;

            if (roll <= cumulative)
                return item;
        }

        return weights[^1];
    }

    private static string GetNeighborArea(string current)
    {
        int index = Array.IndexOf(areas, current);
        if (index < 0)
            return areas[UnityEngine.Random.Range(0, areas.Length)];

        // Roll 0..99
        int roll = UnityEngine.Random.Range(0, 100);

        // 60% chance: same area
        if (roll < 60)
            return current;

        // otherwise: 40% split to 20% left, 20% right    
        int leftIndex = (index - 1 + areas.Length) % areas.Length;
        int rightIndex = (index + 1) % areas.Length;

        // 20% left (roll < 80), 20% right (roll >= 80)
        if (roll < 80)
            return areas[leftIndex];
        else
            return areas[rightIndex];
    }

    private static string GetRandomArea()
    {
        return areas[UnityEngine.Random.Range(0, areas.Length)];
    }
}
