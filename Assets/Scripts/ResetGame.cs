using TransitionSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour
{
    public static ResetGame Instance;
    private GameObject gameManagerObject;
    private GameObject impManagerObject;
    public bool paused;

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
        if (!TransitionManager.GetInstance().runningTransition && !paused && Input.GetKeyDown(KeyCode.Escape))
        {
            paused = true;
            Time.timeScale = 0;
            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        }
        else if (paused && Input.GetKeyDown(KeyCode.Escape))
        {
            paused = false;
            Time.timeScale = 1;
            SceneManager.UnloadSceneAsync("PauseMenu");
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
            impManagerObject = GameObject.FindGameObjectWithTag("ImpManager");
            SceneTransition.Load("StartScreen");
            Destroy(gameManagerObject);
            Destroy(impManagerObject);
            Destroy(gameObject);
        }
    }
}
