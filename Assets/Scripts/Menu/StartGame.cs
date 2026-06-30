using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class StartGame : MonoBehaviour
{
    private TutorialInitiater tutorial;
    private GameObject gameManagerObject;
    private GameObject impManagerObject;


    private void Awake()
    {
        gameManagerObject = GameObject.FindGameObjectWithTag("GameManager");
        impManagerObject = GameObject.FindGameObjectWithTag("ImpManager");
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("MainMenu");
        Destroy(gameManagerObject);
        Destroy(impManagerObject);
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

        SceneManager.LoadScene("Tutorial");
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    public void GoToStartScreen()
    {
        SceneManager.LoadScene("StartScreen");
    }

}
