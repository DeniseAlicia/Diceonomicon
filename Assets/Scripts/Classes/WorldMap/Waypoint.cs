using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using Unity.Mathematics;

[RequireComponent(typeof(Collider))]
public class Waypoint : MonoBehaviour
{
    [Header("Attributes")]
    public bool startingNode;
    public bool isBlocked = false;

    [Header("Info")]
    public string nodeID;
    public string parentID;
    public Vector3 position;
    public int level;
    public string area;
    public int colorIndex;
    public string type;
    private int growthCount = 0;
    public WaypointData data;
    public Renderer waypointIcon;

    public bool hasBranched;

    public List<Vector3> curvePoints = new List<Vector3>();

    [Header("Details")]
    public Color[] colors = new Color[]
               {
        Color.red,
        Color.blue,
        Color.green,
        new(0.6f, 0f, 0.7f),
        Color.white,
        Color.yellow
               };
    public string[] areas = { "Red", "Blue", "Green", "Purple", "White", "Yellow" };

    [Header("References")]
    private Transform centralCylinder;
    private CameraOrbitController cameraController;
    public Material pathMaterial;
    public MeshRenderer pathMeshRenderer;
    public Renderer nodeRenderer;
    private MapManager mapManager;
    private GameStateManager mapStateManager;

    [Header("Spawn / Spiral")]
    public float coreRadius = 3f;               // radius of central cylinder surface
    public static float globalSpiralAngle = 25f; // degrees, increments per spawn to form spiral
    public float spiralStepDeg = 0f;           // how much the spiral advances each time this node spawns

    [Header("Cluster Layout")]
    public float sideRadiusDelta = 0.1f;       // small radius delta for the two upper branches
    public float upperYOffset = -0.7f;             // upper branches same Y as clicked (so 0)
    public float lowerYOffset = -1.4f;          // lower branch Y relative to clicked
    public float radialOffsetOutwards = 0.5f;  // how much mid control is pulled outward for curve

    [Header("Tube / Growth")]
    public GameObject nodePrefab;
    public float tubeRadius = 1f;
    public int tubeSegments = 16;
    public float growthDuration = 1.0f;
    public int bezierSamples = 36;

    [Header("Angle tweak")]
    public float smallAnglePairSeparation = 10.0f; // degrees offset to separate upper branches sideways

    // Hold-to-spawn variables
    private Coroutine holdCoroutine;
    private bool isHolding = false;
    private float holdDuration = 1f;

    public enum WaypointType
    {
        Battle,
        Dice,
        Candlemaker,
    }

    Dictionary<WaypointType, float> waypointWeights = new Dictionary<WaypointType, float>()
    {
        { WaypointType.Battle, 20f },
        { WaypointType.Dice, 40f },
        { WaypointType.Candlemaker, 40f },
    };


    void Start()
    {
        cameraController = FindFirstObjectByType<CameraOrbitController>();
        centralCylinder = cameraController.centerTransform;
        mapManager = FindFirstObjectByType<MapManager>();

        SetWaypointData();
        ApplyColor();
        OrientOutward();

        isBlocked = false;

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.RegisterNode(gameObject, null);
        }

        if (GameStateManager.Instance.battleWon && nodeID == GameStateManager.Instance.pendingBranchNodeId)
        {
            SpawnCluster();
            Debug.Log("Spawning");
        }
        GameStateManager.Instance.RebuildTube(this);
    }

    public Vector3 GetBranchDirection()
    {
        Vector3 dir = transform.position - centralCylinder.position;
        dir.y = 0f;
        return dir.normalized;
    }

    public void SetWaypointData()
    {
        if (type == "")
        {
            GetRandomWaypoint();
        }

        if (data == null)
        {
            data = Resources.Load<WaypointData>($"Waypoints/{type}" + "WaypointData");
        }
        ;

        if (string.IsNullOrEmpty(nodeID))
        {
            nodeID = System.Guid.NewGuid().ToString();
        }

        if (data.waypointArt != null)
        {
            waypointIcon.material.mainTexture = data.waypointArt;
        }
    }

    public void GetRandomWaypoint()
    {
        WaypointType randomType = GetWeightedRandom(waypointWeights);
        type = randomType.ToString();

        if (type != null)
        {
            string dataName = type + "WaypointData";
            data = Resources.Load<WaypointData>($"Waypoints/{dataName}");
        }
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (!hasBranched && !isHolding && !isBlocked)
        {
            isHolding = true;
            holdCoroutine = StartCoroutine(HoldAndSpawn());
        }
    }

    private void OnMouseUp()
    {
        if (isHolding)
        {
            isHolding = false;
            if (holdCoroutine != null)
            {
                StopCoroutine(holdCoroutine);
            }
        }
    }

    public IEnumerator HoldAndSpawn()
    {
        float timer = 0f;
        while (timer < holdDuration)
        {
            if (!isHolding)
            {
                yield break;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        if (!hasBranched)
        {
            Waypoint instance = this;
            hasBranched = true;
            GameStateManager.Instance.player.type = this.type;
            // Debug.Log(hasBranched.ToString());
            SaveMapDebounced();
            GameStateManager.Instance.TriggerWaypoint(instance);
        }
        isHolding = false;
    }

    public void SpawnCluster()
    {
        if (centralCylinder == null)
        {
            cameraController = FindFirstObjectByType<CameraOrbitController>();
            centralCylinder = cameraController.centerTransform;
            mapManager = FindFirstObjectByType<MapManager>();
        }

        // base angle: use node's current angle around center (so cluster aligns to node)
        Vector3 dir = transform.position - centralCylinder.position;
        dir.y = 0f;
        float baseAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        // Also incorporate global spiral offset so clusters advance
        float clusterAngle = baseAngle + globalSpiralAngle;

        // upper branches (same Y as clicked)
        float upperY = transform.position.y + upperYOffset * 2;
        float upperY2 = transform.position.y - upperYOffset;
        float lowerY = transform.position.y + lowerYOffset;

        // Convert to radians
        float baseRad = clusterAngle * Mathf.Deg2Rad;
        float angleOffsetRad = smallAnglePairSeparation * Mathf.Deg2Rad;

        // Two upper branches: slightly separated by angle and different radii
        Vector3 endUpperA = PointOnCore(baseRad + angleOffsetRad, coreRadius + sideRadiusDelta, upperY);
        Vector3 endUpperB = PointOnCore(baseRad - angleOffsetRad, coreRadius - sideRadiusDelta, upperY2);

        // Lower branch: same angle but lower Y, choose coreRadius base
        Vector3 endLower = PointOnCore(baseRad, coreRadius, lowerY);

        // midpoints pulled outward for nicer curvature
        Vector3 midA = MidpointPulled(transform.position, endUpperA);
        Vector3 midB = MidpointPulled(transform.position, endUpperB);
        Vector3 midC = MidpointPulled(transform.position, endLower);

        // animate three branches (spawn nodes at tips but deactivated until grown)
        StartCoroutine(AnimateAndSpawn(transform.position, midA, endUpperA));
        StartCoroutine(AnimateAndSpawn(transform.position, midB, endUpperB));
        StartCoroutine(AnimateAndSpawn(transform.position, midC, endLower));
    }

    Vector3 PointOnCore(float angleRad, float radius, float y)
    {
        return new Vector3(
            centralCylinder.position.x + Mathf.Cos(angleRad) * radius,
            y,
            centralCylinder.position.z + Mathf.Sin(angleRad) * radius
        );
    }

    Vector3 MidpointPulled(Vector3 start, Vector3 end)
    {
        Vector3 mid = (start + end) * 0.5f;
        Vector3 outward = (end - centralCylinder.position).normalized;
        mid += outward * radialOffsetOutwards;
        // small random jitter for organic look (tiny)
        mid += UnityEngine.Random.insideUnitSphere * 0.02f;
        return mid;
    }

    IEnumerator AnimateAndSpawn(Vector3 p0, Vector3 p1, Vector3 p2)
    {
        // instantiate node at p2, set inactive until tube completes
        GameObject nextNode = Instantiate(nodePrefab, p2, Quaternion.identity);
        float sameColorProbability = 0.8f;

        // copy settings to child
        Waypoint waypoint = nextNode.GetComponent<Waypoint>();
        if (waypoint != null)
        {
            CopySettingsTo(waypoint);

            waypoint.InitializeNewNode(this);
        }

        if (mapManager != null)
        {
            // Decide whether to keep same color or shift ±1
            int newIndex;
            if (UnityEngine.Random.value < sameColorProbability)
            {
                // Same color
                newIndex = colorIndex;
            }
            else
            {
                // ±1 index shift
                int offset = UnityEngine.Random.value < 0.5f ? -1 : 1;
                newIndex = Mathf.Clamp(colorIndex + offset, 0, colors.Length - 1);
            }

            float diff = p2.y - transform.position.y;
            Debug.Log(diff.ToString());

            waypoint.level = (int)math.ceil(transform.position.y / -4);

            waypoint.hasBranched = false;
            waypoint.colorIndex = newIndex;
            waypoint.ApplyColor();
            
            cameraController.wpTransform = waypoint.transform;
        }

        nextNode.SetActive(false);

        // create tube go
        GameObject tubeObj = new GameObject("GrowingTube");
        MeshFilter mf = tubeObj.AddComponent<MeshFilter>();
        MeshRenderer mr = tubeObj.AddComponent<MeshRenderer>();
        waypoint.pathMeshRenderer = mr;
        waypoint.pathMeshRenderer.material = new Material(pathMaterial);
        mr.material.color = waypoint.colors[colorIndex];


        // precompute full bezier curve
        Vector3[] full = new Vector3[bezierSamples];
        for (int i = 0; i < bezierSamples; i++)
        {
            float t = i / (float)(bezierSamples - 1); full[i] = Bezier(p0, p1, p2, t);
        }

        float timer = 0f;
        while (timer < growthDuration)
        {
            timer += Time.deltaTime;
            float prog = Mathf.Clamp01(timer / growthDuration);
            int visible = Mathf.Max(2, Mathf.RoundToInt(prog * bezierSamples));
            Vector3[] partial = new Vector3[visible];
            System.Array.Copy(full, 0, partial, 0, visible);

            Mesh mesh = TubeMeshBuilder.BuildTube(partial, tubeRadius, tubeSegments);
            mf.mesh = mesh;

            yield return null;
        }

        // finalize
        mf.mesh = TubeMeshBuilder.BuildTube(full, tubeRadius, tubeSegments);

        // orient the spawned node so its flat face points outward
        if (waypoint != null)
        {
            waypoint.OrientOutward();
            waypoint.parentID = this.nodeID;
            waypoint.curvePoints = new List<Vector3>(full);
        }
        // Ensure child is registered!
        GameStateManager.Instance.RegisterNode(nextNode, nodeID);

        // Mark latest for camera restore
        GameStateManager.Instance.lastWaypoint = waypoint.nodeID;

        // Save immediately when cluster completes
        Invoke(nameof(SaveMapDebounced), 0.5f);
        nextNode.SetActive(true);
        tubeObj.name = "Pathway";

        //----------- SAVE SYSTEM NEW -----------

        // Register this child node
        if (GameStateManager.Instance != null)
        {
            Waypoint childIdent = nextNode.GetComponent<Waypoint>();
            if (string.IsNullOrEmpty(childIdent.nodeID))
                childIdent.nodeID = System.Guid.NewGuid().ToString();

            GameStateManager.Instance.RegisterNode(nextNode, null);

            // Count branch completions
            growthCount++;
            if (growthCount >= 3) // all 3 finished
            {
                // Mark middle child as camera focus target on load
                // MapStateManager.Instance.lastSpawnedNodeId = childIdent.nodeID;

                // Save full state to disk once per cluster
                GameStateManager.Instance.SaveToDisk();
                Debug.Log("Cluster saved! Nodes: " + growthCount);
            }
        }
        //---------------------------------------

        Transform tubesRoot = GameObject.Find("Tubes")?.transform;
        if (tubesRoot == null)
        {
            GameObject go = new GameObject("Tubes");
            tubesRoot = go.transform;
        }
        tubeObj.transform.SetParent(tubesRoot);
    }

    void CopySettingsTo(Waypoint waypoint)
    {
        waypoint.centralCylinder = centralCylinder;
        waypoint.cameraController = cameraController;
        waypoint.coreRadius = coreRadius;
        waypoint.spiralStepDeg = spiralStepDeg;
        waypoint.sideRadiusDelta = sideRadiusDelta;
        waypoint.upperYOffset = upperYOffset;
        waypoint.lowerYOffset = lowerYOffset;
        waypoint.radialOffsetOutwards = radialOffsetOutwards;
        waypoint.nodePrefab = nodePrefab;
        waypoint.tubeRadius = tubeRadius;
        waypoint.tubeSegments = tubeSegments;
        waypoint.growthDuration = growthDuration;
        waypoint.bezierSamples = bezierSamples;
        waypoint.smallAnglePairSeparation = smallAnglePairSeparation;
        waypoint.mapManager = mapManager;
        waypoint.colorIndex = colorIndex;
        waypoint.ApplyColor();
    }

    Vector3 Bezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        float u = 1 - t;
        return u * u * a + 2f * u * t * b + t * t * c;
    }

    public void OrientOutward()
    {
        if (centralCylinder == null) return;

        Vector3 outward = transform.position - centralCylinder.position;
        if (outward.sqrMagnitude < 1e-6f) return;

        outward.y = 0f;
        outward.Normalize();
        transform.rotation = Quaternion.LookRotation(outward, Vector3.forward);
        transform.Rotate(0f, -90f, 0f, Space.World);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -90f);
    }

    public void ApplyColor()
    {
        if (nodeRenderer == null || mapManager == null || GameStateManager.Instance == null)
        {
            return;
        }

        Debug.Log(GameStateManager.Instance.waypoints.Count.ToString());
        if (GameStateManager.Instance.waypoints.Count < 6)
        {
            colorIndex = GameStateManager.Instance.waypoints.Count;
            nodeRenderer.material.color = colors[colorIndex];
            area = areas[colorIndex];
        }
        else
        {
            nodeRenderer.material.color = colors[colorIndex];
            area = areas[colorIndex];
        }
    }

    public void InitializeNewNode(Waypoint parent)
    {
        nodeID = System.Guid.NewGuid().ToString();
        parentID = parent != null ? parent.nodeID : null;
        GetRandomWaypoint();
        SetWaypointData();
        ApplyColor();
        OrientOutward();
        GameStateManager.Instance.RegisterNode(gameObject, parentID);
    }

    void SaveMapDebounced()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SaveToDisk();
    }

    public void LoadFromData(WaypointSaveData data)
    {
        nodeID = data.nodeID;
        parentID = data.parentID;
        position = transform.position = data.position;
        transform.rotation = data.rotation;
        level = data.level;
        area = data.area;
        colorIndex = data.colorIndex;
        curvePoints = new List<Vector3>(data.curvePoints);
        hasBranched = data.hasBranched;

        ApplyColor();
        OrientOutward();
    }

    public T GetWeightedRandom<T>(Dictionary<T, float> weights)
    {
        float totalWeight = 0f;
        foreach (var w in weights.Values)
            totalWeight += w;

        float roll = UnityEngine.Random.value;

        float cumulative = 0f;

        foreach (var kvp in weights)
        {
            float normalized = kvp.Value / totalWeight;
            cumulative += normalized;

            if (roll <= cumulative)
                return kvp.Key;
        }

        return default;
    }


}
