using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    private ResetGame Instance;
    private GameObject gameManagerObject;
    private GameObject impManagerObject;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
            impManagerObject = GameObject.FindGameObjectWithTag("ImpManager");
            SceneManager.LoadScene("StartScreen");
            Destroy(gameManagerObject);
            Destroy(impManagerObject);
            Destroy(gameObject);
        }
    }
}
