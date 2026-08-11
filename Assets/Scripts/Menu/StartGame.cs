using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TransitionSystem;
using UnityEngine.Playables;
using System.Collections;

public class StartGame : MonoBehaviour
{
    private GameObject gameManagerObject;
    private GameObject impManagerObject;
    [SerializeField] public TransitionSettings transition;
    [SerializeField] private PlayableDirector director;
    private float interval = 340f; // 5 minutes

    private Coroutine timelineCoroutine;

    private void Awake()
    {

        gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        impManagerObject = GameObject.FindGameObjectWithTag("ImpManager");
        TransitionManager.GetInstance().runningTransition = false;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "StartScreen" && Input.GetKeyDown(KeyCode.Escape))
        {
            CancelTimeline();
        }
    }


    private void Start()
    {
        Application.targetFrameRate = 60;
        if (SceneManager.GetActiveScene().name == "StartScreen")
        {
            timelineCoroutine = StartCoroutine(RepeatTimeline());
        }
    }

    public void StartNewGame()
    {
        Destroy(gameManagerObject);
        Destroy(impManagerObject);
        SceneTransition.Load("MainMenu");
        //SceneManager.LoadScene("MainMenu");
    }

    public void StartTutorial()
    {
        GameStateManager.Instance.player.area = "Tutorial";
        GameStateManager.Instance.player.level = 0;

        GameStateManager.Instance.player.activeImplings = new List<TabletData>();
        TabletData tutorialImpling = Resources.Load<TabletData>($"Implings/TutorialData");
        GameStateManager.Instance.player.diceDeck = new List<DiceData>();
        GameStateManager.Instance.CreateDiceDeck();
        GameStateManager.Instance.player.activeImplings.Add(tutorialImpling);

        SceneTransition.Load("Tutorial");
        //SceneManager.LoadScene("Tutorial");
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    public void OnResumeButtonPressed()
    {
        ResetGame.Instance.paused = false;
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("PauseMenu");
    }

    public void GoToStartScreen()
    {
        ResetGame.Instance.paused = false;
        Time.timeScale = 1;
        SceneTransition.Load("StartScreen");
        //SceneManager.LoadScene("StartScreen");
    }

    private void CancelTimeline()
    {
        Debug.Log("Timeline cancelled!");

        // Stop playback
        director.Stop();

        // Jump back to beginning
        director.time = 0;
        director.Evaluate();
    }

    private void OnDisable()
    {
        if (director != null)
        {
            director.Stop();
            director.time = 0;
            director.Evaluate();
        }
    }

    private IEnumerator RepeatTimeline()
    {
        while (true)
        {
            Debug.Log("Waiting " + interval + " seconds...");

            yield return new WaitForSecondsRealtime(interval);

            Debug.Log("Playing Timeline!");

            director.time = 0;
            director.Evaluate();
            director.Play();

            Debug.Log("Timeline state: " + director.state);
        }
    }
}
