using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Encounters
{
    private static Dictionary<(int level, string area), List<List<string>>> encounterTable
        = new Dictionary<(int, string), List<List<string>>>()
        {
         { (0, "None"), new List<List<string>>
            {
                new List<string>{ "Diedra", "Diedra", "Diedra" },
            }
        },
        { (1, "Green"), new List<List<string>>
            {
                new List<string>{ "Greemlin", "Toksick"},
                new List<string>{ "Diedra", "Diedra" },
                new List<string>{ "Greemlin", "Greemlin"},
                new List<string>{ "Toksick", "Toksick" },
                new List<string>{ "Diedra", "Toksick" },
                new List<string>{ "Greemlin", "Diedra"}
            }
        },
    };

    public Dictionary<string, TabletData> enemyData = new Dictionary<string, TabletData>();

    public static List<TabletData> SetEnemyRoster(int level, string area)
    {
        List<TabletData> tablets = new List<TabletData>();

        if (!encounterTable.TryGetValue((level, area), out var encounterList))
        {
            encounterList = encounterTable[(0, "None")];
        }

        int randomIndex = UnityEngine.Random.Range(0, encounterList.Count);
        List<string> encounter = encounterList[randomIndex];

        foreach (var impName in encounter)
        {
            string dataName = impName + "Data";

            TabletData data = Resources.Load<TabletData>($"Opponents/{dataName}");
            if (data != null)
            {
                tablets.Add(data);
            }
            else
            {
                Debug.LogWarning($"Could not load TabletData for {impName} (looked for {dataName})");
            }
        }
        return tablets;
    }
}