using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    [Header("File Settings")]
    public string saveFileName = "savedata.json";

    [Header("Prefabs & Materials")]
    public GameObject waypointPrefab;
    public Material tubeMaterial;

    [Serializable]
    public class PlayerData
    {
        public string playerID;
        public int level;
        public string area;
        public int colorIndex;
        public string type;
        public int currentHealth;
        public int maxHealth;
        public List<DiceData> diceDeck = new();
        public List<string> implings = new();
        public List<TabletData> activeImplings = new();
        public List<TabletData> unlockedImplings = new();
        public List<RelicData> relics = new();
    }

    [Serializable]
    public class WaypointSaveDataList
    {
        public List<WaypointSaveData> waypoints = new();
    }

    [Header("Runtime State")]
    public PlayerData player = new();
    public Dictionary<string, GameObject> waypoints = new();
    public ImpSelectManager impSelect;

    [Header("Map Progress")]
    public string lastWaypoint;
    public bool battleWon;
    public string pendingBranchNodeId;
    public Waypoint tempWp;
    private MapSaveData cachedMapData;

    private List<WaypointSaveData> loadedData;
    private readonly int radius = 6;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        impSelect = FindAnyObjectByType<ImpSelectManager>();
        if (impSelect.newGame == true)
        {
            impSelect.newGame = false;
            ResetSave();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void Start()
    {
        if (!HasSaveFile())
        {
            Debug.LogWarning("No save found. Starting new map!");
            ResetSave();  // will spawn new default nodes
        }
        else
        {
            LoadFromDisk();
        }
    }

    private List<TabletData> SetImplingRoster()
    {
        List<TabletData> list = new();
        foreach (TabletData impName in impSelect.selectedImplings)
        {
            // string dataName = impName + "Data";
            // TabletData data = Resources.Load<TabletData>($"Implings/{dataName}");
            TabletData data = impName;
            if (data != null)
                list.Add(data);
        }
        return list;
    }

    public void CreateDiceDeck()
    {
        player.diceDeck.Clear();
        foreach (TabletData impling in player.activeImplings)
        {
            for (int i = 0; i < impling.startingDice.Length; i++)
            {
                player.diceDeck.Add(impling.startingDice[i]);
            }
        }
    }

    //─────────────────────────────────────────────
    // MAP FUNCTIONS
    //─────────────────────────────────────────────
    public void TriggerWaypoint(Waypoint waypoint)
    {
        player.level = waypoint.level;
        player.area = waypoint.area;
        lastWaypoint = waypoint.nodeID;
        pendingBranchNodeId = waypoint.nodeID;
        waypoint.hasBranched = true;

        tempWp = waypoint;
        BlockPaths();
        waypoint.data.DoEffect();

        // SceneManager.LoadScene("BattleSetup", LoadSceneMode.Single);
    }

    // NEW overload that includes parent linkage
    public void RegisterNode(GameObject node, string parentID)
    {
        if (node == null) return;
        Waypoint wp = node.GetComponent<Waypoint>();
        if (wp == null) return;

        if (string.IsNullOrEmpty(wp.nodeID))
            wp.nodeID = Guid.NewGuid().ToString();

        wp.parentID = parentID;

        if (!waypoints.ContainsKey(wp.nodeID))
        {
            waypoints.Add(wp.nodeID, node);
        }
        else
        {
            waypoints[wp.nodeID] = node;
        }
    }

    public void RebuildTube(Waypoint wp)
    {
        if (wp.curvePoints == null || wp.curvePoints.Count < 3) return;

        GameObject tubeObj = new GameObject("Pathway");
        MeshFilter mf = tubeObj.AddComponent<MeshFilter>();
        MeshRenderer mr = tubeObj.AddComponent<MeshRenderer>();
        mr.material = tubeMaterial;

        mf.mesh = TubeMeshBuilder.BuildTube(wp.curvePoints.ToArray(), wp.tubeRadius, wp.tubeSegments);

        Transform tubesRoot = GameObject.Find("Tubes")?.transform;
        if (tubesRoot == null)
        {
            GameObject go = new GameObject("Tubes");
            tubesRoot = go.transform;
        }
        tubeObj.transform.SetParent(tubesRoot);
    }

    public void BlockPaths()
    {
        Waypoint[] allWaypoints = FindObjectsByType<Waypoint>(FindObjectsSortMode.None);

        foreach (Waypoint wp in allWaypoints)
        {
            wp.isBlocked = true;
            wp.nodeRenderer.material.color = Color.gray5;
        }

    }

    //─────────────────────────────────────────────
    // SAVE / LOAD SYSTEM
    //─────────────────────────────────────────────
    public bool HasSaveFile() =>
        File.Exists(Path.Combine(Application.persistentDataPath, saveFileName));

    public void SaveToDisk()
    {
        SaveDataContainer container = new SaveDataContainer();

        // Player data always saved
        container.playerData = new PlayerSaveData
        {
            playerID = player.playerID,
            level = player.level,
            area = player.area,
            currentHealth = player.currentHealth,
            maxHealth = player.maxHealth,
            diceDeck = player.diceDeck,
            implings = player.implings,
            unlockedImplings = player.unlockedImplings,
            activeImplings = player.activeImplings,
        };

        if (SceneManager.GetActiveScene().name.Equals("Map", StringComparison.OrdinalIgnoreCase))
        {
            // Save waypoints only if on map scene
            MapSaveData mapData = new MapSaveData
            {
                lastWaypoint = lastWaypoint,
                pendingBranchNodeId = pendingBranchNodeId,
                battleWon = battleWon,
                nodes = new List<WaypointSaveData>()
            };

            foreach (var kvp in waypoints)
            {
                var obj = kvp.Value;
                if (obj == null)
                {
                    Debug.LogWarning($"Waypoint {kvp.Key} object is null!");
                    continue;
                }

                Waypoint wp = obj.GetComponent<Waypoint>();
                if (wp == null)
                {
                    Debug.LogWarning($"Waypoint {kvp.Key} missing Waypoint component!");
                    continue;
                }

                WaypointSaveData nodeData = new WaypointSaveData
                {
                    nodeID = wp.nodeID,
                    level = wp.level,
                    area = wp.area,
                    colorIndex = wp.colorIndex,
                    type = wp.type,
                    position = wp.transform.position,
                    rotation = wp.transform.rotation,
                    parentID = wp.parentID,
                    curvePoints = new List<Vector3>(wp.curvePoints),
                    hasBranched = wp.hasBranched
                };
                mapData.nodes.Add(nodeData);
            }

            cachedMapData = mapData;

            container.mapData = mapData; // **Assign mapData before saving**
        }
        else
        {
            container.mapData = cachedMapData; // Use cached data outside map scene
        }

        // Serialize entire container to JSON string and save every time
        string json = JsonUtility.ToJson(container, true);
        string path = Path.Combine(Application.persistentDataPath, saveFileName);
        File.WriteAllText(path, json);

        //Debug.Log($"Saved game with {container.mapData?.nodes?.Count ?? 0} waypoints and player data to {path}");
    }

    public void LoadFromDisk()
    {
        string path = Path.Combine(Application.persistentDataPath, saveFileName);
        if (!File.Exists(path))
        {
            Debug.LogWarning("Save file not found!");
            return;
        }

        string json = File.ReadAllText(path);
        SaveDataContainer container = JsonUtility.FromJson<SaveDataContainer>(json);

        if (container == null)
        {
            Debug.LogWarning("Failed to load save data!");
            return;
        }

        // Restore player data
        PlayerSaveData pData = container.playerData;
        player.playerID = pData.playerID;
        player.level = pData.level;
        player.area = pData.area;
        player.currentHealth = pData.currentHealth;
        player.maxHealth = pData.maxHealth;
        player.diceDeck = pData.diceDeck;
        player.implings = pData.implings;
        player.unlockedImplings = pData.unlockedImplings;
        player.activeImplings = pData.activeImplings;

        // Clear old waypoints
        foreach (var wpObj in waypoints.Values)
            if (wpObj != null) Destroy(wpObj);
        waypoints.Clear();

        // Restore map data
        MapSaveData mapData = container.mapData;
        lastWaypoint = mapData.lastWaypoint;

        foreach (WaypointSaveData nodeData in mapData.nodes)
        {
            GameObject wpObj = Instantiate(waypointPrefab, nodeData.position, nodeData.rotation);
            Waypoint wp = wpObj.GetComponent<Waypoint>();
            wp.nodeID = nodeData.nodeID;
            wp.level = nodeData.level;
            wp.area = nodeData.area;
            wp.colorIndex = nodeData.colorIndex;
            wp.type = nodeData.type;
            wp.parentID = nodeData.parentID;
            wp.curvePoints = new List<Vector3>(nodeData.curvePoints);
            RegisterNode(wpObj, wp.parentID);
            wp.hasBranched = nodeData.hasBranched;
        }

        Debug.Log($"Loaded game with {mapData.nodes.Count} waypoints.");
    }

    //─────────────────────────────────────────────
    // AUTO-SAVE
    //─────────────────────────────────────────────

    private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        if (!oldScene.IsValid())
            return;

        if (oldScene.name.Equals("Map", StringComparison.OrdinalIgnoreCase))
        {
            if (waypoints != null && waypoints.Count > 0)
            {
                SaveToDisk();
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("Map", StringComparison.OrdinalIgnoreCase))
        {
            LoadFromDisk();
        }
    }

    public void OnBattleEnd()
    {
        battleWon = true;
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            player.activeImplings = SetImplingRoster();
            CreateDiceDeck();
        }
        
        SaveToDisk();
    }

    public void ResetSave()
    {
        string path = Path.Combine(Application.persistentDataPath, saveFileName);
        if (File.Exists(path)) File.Delete(path);

        waypoints.Clear();
        loadedData?.Clear();

        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene.Equals("Map", StringComparison.OrdinalIgnoreCase))
        {
            foreach (Waypoint obj in FindObjectsByType<Waypoint>(FindObjectsSortMode.None))
                Destroy(obj.gameObject);
        }

        player = new PlayerData
        {
            playerID = Guid.NewGuid().ToString(),
            level = 0,
            area = "None",
            colorIndex = 0,
            maxHealth = 100,
            currentHealth = 100,
            implings = new List<string> { "Beempling", "Hie", "Cubie" },
            activeImplings = new List<TabletData>(),
            unlockedImplings = new List<TabletData>(),
            diceDeck = new List<DiceData>()
        };

        if (player.activeImplings.Count == 0)
        {
            player.activeImplings = SetImplingRoster();
        }
        CreateDiceDeck();

        lastWaypoint = null;
        pendingBranchNodeId = null;
        battleWon = false;

        if (currentScene.Equals("Map", StringComparison.OrdinalIgnoreCase))
        {
            StartCoroutine(SpawnAndSaveNextFrame());
        }
        else
        {
            SaveToDisk();
        }
        Debug.Log("Reset complete.");
    }

    private IEnumerator SpawnAndSaveNextFrame()
    {
        yield return null; // wait one frame
        SpawnPrefabs();

        // Wait for some time or frames to let waypoints finish their Init
        yield return new WaitForSeconds(0.5f);

        SaveToDisk();
    }

    void SpawnPrefabs()
    {
        GameObject container = new GameObject("SpawnedWaypoints");

        float angleStep = 360f / 6;
        for (int i = 0; i < 6; i++)
        {
            float angle = i * angleStep;
            Vector3 pos = transform.position + new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                0,
                Mathf.Sin(angle * Mathf.Deg2Rad) * radius
            );

            GameObject instance = Instantiate(waypointPrefab, pos, Quaternion.identity, container.transform);

            if (instance.TryGetComponent(out Waypoint wp))
            {
                wp.SetWaypointData();
                wp.colorIndex = i;
                wp.level = 0;
                RegisterNode(instance, null);
            }
        }
    }
}
