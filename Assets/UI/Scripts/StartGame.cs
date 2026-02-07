using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;

public class StartGame : MonoBehaviour
{
    public void StartGameToMainMenu()
    {
        UIManager.menuToOpenOnLoad = MenuType.Main;
        SceneManager.LoadScene("Main_Menu");
    }

    public void StartGameToSettings()
    {
        UIManager.menuToOpenOnLoad = MenuType.Settings;
        SceneManager.LoadScene("Main_Menu");
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
