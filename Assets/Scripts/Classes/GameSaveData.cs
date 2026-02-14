using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SaveDataContainer
{
    public PlayerSaveData playerData;
    public MapSaveData mapData;
}

[Serializable]
public class WaypointSaveData
{
    public string nodeID;
    public int level;
    public string area;
    public int colorIndex;
    public string type;
    public Vector3 position;
    public Quaternion rotation;
    public string parentID;
    public List<Vector3> curvePoints = new List<Vector3>();
    public bool hasBranched;
}

[Serializable]
public class MapSaveData
{
    public List<WaypointSaveData> nodes = new();
    public string lastWaypoint;
    public bool battleWon;
    public string pendingBranchNodeId;
}

[Serializable]
public class PlayerSaveData
{
    // General Info
    public string playerID;
    public int level;
    public string area;

    // Battle Info
    public int currentHealth;
    public int maxHealth;
    public List<DiceData> diceDeck;
    public List<string> implings;
    public List<TabletData> unlockedImplings;
    public List<TabletData> activeImplings;
    public List<RelicData> relics;

    // Map Info
    public string mapSaveData;
}