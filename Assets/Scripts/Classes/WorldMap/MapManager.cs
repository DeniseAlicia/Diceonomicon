using UnityEngine.SceneManagement;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject prefab;      // The prefab to spawn
    public int numberOfPrefabs = 6; // Number of prefabs to spawn
    public float radius = 5f;       // Radius of the circle

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            MapStateManager.Instance.SaveToDisk();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            MapStateManager.Instance.LoadFromDisk();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("BattleSetup", LoadSceneMode.Single);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            MapStateManager.Instance.ResetSave();
            SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
            );
        }
    }

    void Start()
    {
        if (MapStateManager.Instance != null && MapStateManager.Instance.HasSaveFile())
        {
            MapStateManager.Instance.LoadFromDisk();
        }
        else
        {
            SpawnPrefabs();
        }
    }



    void SpawnPrefabs()
    {
        GameObject container = new GameObject("SpawnedWaypoints");

        float angleStep = 360f / numberOfPrefabs;
        for (int i = 0; i < numberOfPrefabs; i++)
        {
            float angle = i * angleStep;
            Vector3 pos = transform.position + new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                0,
                Mathf.Sin(angle * Mathf.Deg2Rad) * radius
            );

            GameObject instance = Instantiate(prefab, pos, Quaternion.identity, container.transform);

            if (instance.TryGetComponent(out Waypoint wp))
            {
                wp.SetWaypointData();
                wp.colorIndex = i;
                wp.level = 0;
                MapStateManager.Instance.RegisterNode(instance);
            }
        }
    }
}
