using UnityEngine.SceneManagement;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject prefab;      // The prefab to spawn
    public int numberOfPrefabs = 6; // Number of prefabs to spawn
    public float radius = 5f;       // Radius of the circle

    [System.Obsolete]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameStateManager.Instance.SaveToDisk();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameStateManager.Instance.LoadFromDisk();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene("BattleSetup", LoadSceneMode.Single);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameStateManager.Instance.ResetSave();
            SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
            );
        }
    }

    void Start()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.HasSaveFile())
        {
            GameStateManager.Instance.LoadFromDisk();
        }
    }
}
