using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndBattle : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject victoryScreen;
    public GameObject buttonContainer;

    public Button restartButton;
    public Button quitButton;

    // Add Event Listener: When player currentHealth =< 0 do:

    // gameOverScreen.FMOD.playEvent();
    public void Win()
    {
        victoryScreen.SetActive(true);

        buttonContainer.SetActive(true);
        Button restart = restartButton.GetComponent<Button>();
        restart.onClick.AddListener(RestartOnClick);

        Button quit = quitButton.GetComponent<Button>();
        quit.onClick.AddListener(QuitOnClick);
    }

    public void Lose()
    {
        gameOverScreen.SetActive(true);

        buttonContainer.SetActive(true);
        Button restart = restartButton.GetComponent<Button>();
        restart.onClick.AddListener(RestartOnClick);

        Button quit = quitButton.GetComponent<Button>();
        quit.onClick.AddListener(QuitOnClick);
    }

    void RestartOnClick()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    void QuitOnClick()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}