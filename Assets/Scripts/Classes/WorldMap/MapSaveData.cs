using UnityEngine;
using System;
using System.Collections.Generic;

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
}


[Serializable]
public class MapSaveData
{
    public List<WaypointSaveData> nodes = new();
    public string lastWaypoint;
}