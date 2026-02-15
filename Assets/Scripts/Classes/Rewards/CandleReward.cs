using UnityEngine;
using System.Collections.Generic;
using System;

public static class CandleReward
{
    public static List<string> options = new();

    // First choice table
    private static readonly List<(string reward, float weight)> healingTable =
        new()
        {
            ("RestoreSmall", 8.9f),
            ("RestoreMedium", 1f),
            ("RestoreLarge", 0.1f),
        };

    // Second choice table
    private static readonly List<(string reward, float weight)> maxTable =
        new()
        {
            ("AddSmallWax", 8.9f),
            ("AddMediumWax", 1f),
            ("AddLargeWax", 0.1f),
        };

    // Third choice table
    private static readonly List<(string reward, float weight)> payTable =
        new()
        {
            ("BurnSmall", 8.9f),
            ("BurnMedium", 1f),
            ("BurnLarge", 0.1f),
        };

    public static void GetRewards()
    {
        options.Clear();

        AddFromTable(healingTable);
        AddFromTable(maxTable);
        AddFromTable(payTable);
    }

    private static void AddFromTable(List<(string reward, float weight)> table)
    {
        var choicePool = new List<(string reward, float weight)>(table);

        // Prevent duplicates across choices
        choicePool.RemoveAll(r => options.Contains(r.reward));

        if (choicePool.Count == 0)
            return;

        var randomizedChoice = GetWeightedRandom(choicePool);
        options.Add(randomizedChoice.reward);
    }

    private static (string reward, float weight) GetWeightedRandom(List<(string reward, float weight)> pool)
    {
        float total = 0f;
        foreach (var item in pool)
            total += item.weight;

        float roll = UnityEngine.Random.value * total;
        float cumulative = 0f;

        foreach (var item in pool)
        {
            cumulative += item.weight;
            if (roll <= cumulative)
                return item;
        }

        return pool[^1];
    }
}