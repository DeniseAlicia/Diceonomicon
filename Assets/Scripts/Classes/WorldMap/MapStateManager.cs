using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapStateManager : MonoBehaviour
{
    public static MapStateManager Instance;

    public string saveFileName = "mapdata.json";
    public GameObject waypointPrefab;
    public Material tubeMaterial;
    public int level;
    public string area;
    public string lastWaypoint;
    public bool battleWon;
    public string pendingBranchNodeId;

    [HideInInspector]
    public string lastSpawnedNodeId = null;

    public Dictionary<string, GameObject> waypoints = new Dictionary<string, GameObject>();
    private List<WaypointSaveData> loadedData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void TriggerWaypoint(Waypoint waypoint)
    {
        level = waypoint.level;
        area = waypoint.area;
        lastWaypoint = waypoint.nodeID;
        pendingBranchNodeId = waypoint.nodeID;
        SceneManager.LoadScene("BattleSetup", LoadSceneMode.Single);
    }

    public bool HasSaveFile()
    {
        return File.Exists(Path.Combine(Application.persistentDataPath, saveFileName));
    }

    // Called by waypoint at creation
    public void RegisterNode(GameObject node, string parentID = null)
    {
        Waypoint waypoint = node.GetComponent<Waypoint>();
        if (waypoint == null || string.IsNullOrEmpty(waypoint.nodeID)) return;

        if (!waypoints.ContainsKey(waypoint.nodeID))
        {
            waypoints.Add(waypoint.nodeID, node);
        }
    }

    public void SaveToDisk()
    {
        List<WaypointSaveData> saveList = new List<WaypointSaveData>();

        foreach (var kvp in waypoints)
        {
            Waypoint waypoint = kvp.Value.GetComponent<Waypoint>();

            WaypointSaveData data = new()
            {
                nodeID = waypoint.nodeID,
                parentID = waypoint.parentID,
                position = waypoint.transform.position,
                rotation = waypoint.transform.rotation,
                level = waypoint.level,
                area = waypoint.area,
                colorIndex = waypoint.colorIndex,
                curvePoints = new List<Vector3>(waypoint.curvePoints)
            };

            saveList.Add(data);
        }

        WaypointSaveDataList container = new WaypointSaveDataList { waypoints = saveList };
        string json = JsonUtility.ToJson(container, true);

        File.WriteAllText(Path.Combine(Application.persistentDataPath, saveFileName), json);
        Debug.Log($"💾 Saved {saveList.Count} waypoints\n{json}");
    }

    public void LoadFromDisk()
    {
        foreach (var wp in waypoints.Values)
        {
            Destroy(wp);
        }
        waypoints.Clear();

        string path = Path.Combine(Application.persistentDataPath, saveFileName);
        if (!File.Exists(path))
        {
            Debug.LogError("❌ No save file found!");
            return;
        }

        string json = File.ReadAllText(path);
        var jsonContainer = JsonUtility.FromJson<WaypointSaveDataList>(json);
        loadedData = jsonContainer.waypoints;

        if (loadedData == null || loadedData.Count == 0)
        {
            Debug.LogError("❌ Save file loaded but contained ZERO nodes!");
            return;
        }

        Debug.Log($"📥 LOADING {loadedData.Count} NODES...");

        // Clear lookup or previous scene content
        waypoints.Clear();

        foreach (var data in loadedData)
        {
            Debug.Log($"➡ Creating Node: {data.nodeID} at {data.position}");

            GameObject node = Instantiate(waypointPrefab, data.position, data.rotation);
            var wp = node.GetComponent<Waypoint>();

            if (wp == null)
            {
                Debug.LogError("❌ Spawned waypointPrefab missing Waypoint component!");
                return;
            }

            wp.LoadFromData(data);

            RegisterNode(node);
        }

        Debug.Log("✅ All nodes spawned — now rebuild tubes…");

        int tubesBuilt = 0;
        foreach (var obj in waypoints.Values)
        {
            var wp = obj.GetComponent<Waypoint>();
            if (!string.IsNullOrEmpty(wp.parentID) &&
                wp.curvePoints != null &&
                wp.curvePoints.Count > 2)
            {
                Debug.Log($"🔗 Rebuilding tube for: {wp.nodeID} -> parent {wp.parentID}");
                RebuildTube(wp);
                tubesBuilt++;
            }
        }

        Debug.Log($"✅ Load complete! Tubes rebuilt: {tubesBuilt}");
        Debug.Log($"📊 NodeLookup after load: {waypoints.Count} entries");

        if (battleWon && !string.IsNullOrEmpty(pendingBranchNodeId))
        {
            if (waypoints.TryGetValue(pendingBranchNodeId, out GameObject node))
            {
                var wp = node.GetComponent<Waypoint>();
                if (wp != null)
                {
                    Debug.Log($"🌱 Spawning cluster from node {pendingBranchNodeId} (battle winner)");
                    battleWon = false;
                    pendingBranchNodeId = null;
                    wp.SpawnCluster(); // call the existing method
                }
            }
            else
            {
                Debug.LogWarning($"⚠️ Could not find waypoint {pendingBranchNodeId} after load.");
            }
        }
    }

    void RebuildTube(Waypoint wp)
    {
        if (wp.curvePoints == null || wp.curvePoints.Count < 3) return;

        GameObject tubeObj = new GameObject("Pathway");
        var mf = tubeObj.AddComponent<MeshFilter>();
        var mr = tubeObj.AddComponent<MeshRenderer>();
        mr.material = tubeMaterial;

        mf.mesh = TubeMeshBuilder.BuildTube(wp.curvePoints.ToArray(), wp.tubeRadius, wp.tubeSegments);

        var parent = GameObject.Find("Pathway");
        if (parent == null) parent = new GameObject("Pathway");
        tubeObj.transform.SetParent(parent.transform);
    }

    [System.Serializable]
    private class WaypointSaveDataList
    {
        public List<WaypointSaveData> waypoints;
    }

    public void ResetSave()
    {
        waypoints.Clear();
        string path = Path.Combine(Application.persistentDataPath, saveFileName);

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("🗑 Save file deleted!");
        }
        else
        {
            Debug.Log("⚠ No save file existed to delete.");
        }
    }
}
