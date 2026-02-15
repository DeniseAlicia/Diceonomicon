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

    public void ContinueGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
