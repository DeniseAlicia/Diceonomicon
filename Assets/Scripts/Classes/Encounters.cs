using UnityEngine;
using System.Collections.Generic;

public class Encounters
{
    private static Dictionary<(int level, string area), List<List<string>>> encounterTable
        = new Dictionary<(int, string), List<List<string>>>()
        {
    // -----------------------------------------------------------------------
    // TUTORIAL
    // -----------------------------------------------------------------------
         { (0, "Tutorial"), new List<List<string>>
            {
                new List<string>{ "TutorialEnemy" },
            }
        },
   // -----------------------------------------------------------------------
    // Green (Buff)
    // -----------------------------------------------------------------------
        { (0, "Green"), new List<List<string>>
            {
                new List<string>{ "Diedra", "Diedra"},
                new List<string>{ "Diedra", "Toksick"},
                new List<string>{ "Toksick", "Diedra"},
                new List<string>{ "Toksick", "Toksick"},
                new List<string>{ "Pierco", "Toksick"},
            }
        },
        { (1, "Green"), new List<List<string>>
            {
                new List<string>{ "Diedra", "Diedra"},
                new List<string>{ "Diedra", "Toksick"},
                new List<string>{ "Toksick", "Diedra"},
                new List<string>{ "Toksick", "Toksick"},
            }
        },
        { (2, "Green"), new List<List<string>>
            {
                new List<string>{ "Diedra", "Toksick", "Diedra"},
            }
        },
        { (3, "Green"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick", "Greemlin"},
            }
        },
        { (4, "Green"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Diedra", "Greemlin"},
            }
        },
        { (5, "Green"), new List<List<string>>
            {
                new List<string>{ "Greemlin", "Toksick"},
            }
        },
        { (6, "Green"), new List<List<string>>
            {
                new List<string>{ "Greemlin", "Greemlin"},
            }
        },
        { (7, "Green"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick"},
            }
        },
        { (8, "Green"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick", "Greemlin"},
            }
        },
        { (9, "Green"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Diedra", "Greemlin"},
            }
        },
    // -----------------------------------------------------------------------
    // Red (Damage)
    // -----------------------------------------------------------------------
         { (0, "Red"), new List<List<string>>
            {
                new List<string>{ "Acula", "Greemlin"},
                new List<string>{ "Dython", "Acula"},
                new List<string>{ "Acula", "Acula"},
            }
        },
        { (1, "Red"), new List<List<string>>
            {
                new List<string>{ "Acula", "Greemlin"},
                new List<string>{ "Dython", "Acula"},
                new List<string>{ "Acula", "Acula"},
            }
        },
        { (2, "Red"), new List<List<string>>
            {
                new List<string>{ "Greemlin", "Maddo", "Greemlin"},
            }
        },
        { (3, "Red"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick", "Greemlin"},
            }
        },
        { (4, "Red"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Diedra", "Greemlin"},
            }
        },
        { (5, "Red"), new List<List<string>>
            {
                new List<string>{ "Greemlin", "Toksick"},
            }
        },
        { (6, "Red"), new List<List<string>>
            {
                new List<string>{ "Greemlin", "Greemlin"},
            }
        },
        { (7, "Red"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick"},
            }
        },
        { (8, "Red"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick", "Greemlin"},
            }
        },
        { (9, "Red"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Diedra", "Greemlin"},
            }
        },
    // -----------------------------------------------------------------------
    // Blue (Block)
    // -----------------------------------------------------------------------
        { (0, "Blue"), new List<List<string>>
            {
                new List<string>{ "Saddie", "Maddo" },
                new List<string>{ "Saddie", "Saddie" },
                new List<string>{ "Maddo", "Saddie" },
                new List<string>{ "Tentice", "Saddie" },
            }
        },
        { (1, "Blue"), new List<List<string>>
            {
                new List<string>{ "Saddie", "Maddo" },
                new List<string>{ "Saddie", "Saddie" },
                new List<string>{ "Maddo", "Saddie" },
                new List<string>{ "Tentice", "Tentice" },
            }
        },
        { (2, "Blue"), new List<List<string>>
            {
                new List<string>{ "Maddo", "Maddo", "Maddo"},
            }
        },
        { (3, "Blue"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick", "Greemlin"},
            }
        },
        { (4, "Blue"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Diedra", "Greemlin"},
            }
        },
        { (5, "Blue"), new List<List<string>>
            {
                new List<string>{ "Greemlin", "Toksick"},
            }
        },
        { (6, "Blue"), new List<List<string>>
            {
                new List<string>{ "Greemlin", "Greemlin"},
            }
        },
        { (7, "Blue"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick"},
            }
        },
        { (8, "Blue"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick", "Greemlin"},
            }
        },
        { (9, "Blue"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Diedra", "Greemlin"},
            }
        },
    // -----------------------------------------------------------------------
    // Purple (Spell)
    // -----------------------------------------------------------------------
        { (0, "Purple"), new List<List<string>>
            {
                new List<string>{ "Greemlin", "Greemlin"},
                new List<string>{ "Dython", "Dython"},
                new List<string>{ "Diedra", "Dython"},
                new List<string>{ "Dython", "Greemlin"},
                new List<string>{ "Sludgeo", "Greemlin"},
                new List<string>{ "Dython", "Sludgeo"},
                new List<string>{ "Sludgeo", "Sludgeo"},
            }
        },
        { (1, "Purple"), new List<List<string>>
            {
                new List<string>{ "Greemlin", "Greemlin"},
                new List<string>{ "Sludgeo", "Sludgeo"},
                new List<string>{ "Diedra", "Dython"},
                new List<string>{ "Sludgeo", "Greemlin"},
                new List<string>{ "Dython", "Sludgeo"},

            }
        },
        { (2, "Purple"), new List<List<string>>
            {
                new List<string>{ "Greemlin", "Toksick", "Diedra"},
            }
        },
        { (3, "Purple"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick", "Greemlin"},
            }
        },
        { (4, "Purple"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Diedra", "Greemlin"},
            }
        },
        { (5, "Purple"), new List<List<string>>
            {
                new List<string>{ "Greemlin", "Toksick"},
            }
        },
        { (6, "Purple"), new List<List<string>>
            {
                new List<string>{ "Greemlin", "Greemlin"},
            }
        },
        { (7, "Purple"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick"},
            }
        },
        { (8, "Purple"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick", "Greemlin"},
            }
        },
        { (9, "Purple"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Diedra", "Greemlin"},
            }
        },
    // -----------------------------------------------------------------------
    // Yellow (Debuff)
    // -----------------------------------------------------------------------
        { (0, "Yellow"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick"},
                new List<string>{ "Diedra", "Toksick"},
                new List<string>{ "Toksick", "Acula"},
                new List<string>{ "Acula", "Sludgeo"},
                new List<string>{ "Acula", "Pierco"},
            }
        },
                { (1, "Yellow"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick"},
                new List<string>{ "Diedra", "Toksick"},
                new List<string>{ "Toksick", "Acula"},
                new List<string>{ "Sludgeo", "Sludgeo"},
            }
        },
        { (2, "Yellow"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick", "Toksick"},
            }
        },
        { (3, "Yellow"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick", "Greemlin"},
            }
        },
        { (4, "Yellow"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Diedra", "Greemlin"},
            }
        },
        { (5, "Yellow"), new List<List<string>>
            {
                new List<string>{ "Greemlin", "Toksick"},
            }
        },
        { (6, "Yellow"), new List<List<string>>
            {
                new List<string>{ "Greemlin", "Greemlin"},
            }
        },
        { (7, "Yellow"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick"},
            }
        },
        { (8, "Yellow"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick", "Greemlin"},
            }
        },
        { (9, "Yellow"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Diedra", "Greemlin"},
            }
        },
    // -----------------------------------------------------------------------
    // White (Neutral)
    // -----------------------------------------------------------------------
        { (0, "White"), new List<List<string>>
            {
                new List<string>{ "Maddo", "Maddo" },
                new List<string>{ "Acula", "Maddo" },
                new List<string>{ "Maddo", "Diedra" },
                new List<string>{ "Pierco", "Maddo" },
                new List<string>{ "Pierco", "Pierco"},

            }
        },
                { (1, "White"), new List<List<string>>
            {
                new List<string>{ "Maddo", "Maddo" },
                new List<string>{ "Maddo", "Acula" },
                new List<string>{ "Diedra", "Maddo" },
                 new List<string>{ "Pierco", "Maddo" },
                new List<string>{ "Pierco", "Pierco"},
            }
        },
        { (2, "White"), new List<List<string>>
            {
                new List<string>{ "Maddo", "Maddo", "Maddo"},
            }
        },
        { (3, "White"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick", "Greemlin"},
            }
        },
        { (4, "White"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Diedra", "Greemlin"},
            }
        },
        { (5, "White"), new List<List<string>>
            {
                new List<string>{ "Greemlin", "Toksick"},
            }
        },
        { (6, "White"), new List<List<string>>
            {
                new List<string>{ "Greemlin", "Greemlin"},
            }
        },
        { (7, "White"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick"},
            }
        },
        { (8, "White"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Toksick", "Greemlin"},
            }
        },
        { (9, "White"), new List<List<string>>
            {
                new List<string>{ "Toksick", "Diedra", "Greemlin"},
            }
        },
    };

    public Dictionary<string, TabletData> enemyData = new Dictionary<string, TabletData>();

    public static List<TabletData> SetEnemyRoster(int level, string area)
    {
        List<TabletData> tablets = new List<TabletData>();

        if (!encounterTable.TryGetValue((level, area), out var encounterList))
        {
            encounterList = encounterTable[(0, "Green")];
        }

        int randomIndex = Random.Range(0, encounterList.Count);
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