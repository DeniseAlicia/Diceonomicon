using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;

public class StartGame : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("MainMenu");
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
}
