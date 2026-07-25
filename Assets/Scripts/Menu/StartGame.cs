using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TransitionSystem;

public class StartGame : MonoBehaviour
{
    private GameObject gameManagerObject;
    private GameObject impManagerObject;
    [SerializeField] public TransitionSettings transition;

    private void Awake()
    {

        gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        impManagerObject = GameObject.FindGameObjectWithTag("ImpManager");
        TransitionManager.GetInstance().runningTransition = false;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
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

}
