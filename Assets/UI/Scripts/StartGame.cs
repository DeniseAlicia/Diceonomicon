using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;

public class StartGame : MonoBehaviour
{
    public void StartGameToMainMenu()
    {
        UIManager.menuToOpenOnLoad = MenuType.Main;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void StartGameToSettings()
    {
        UIManager.menuToOpenOnLoad = MenuType.Settings;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
