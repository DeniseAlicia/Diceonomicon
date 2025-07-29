using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class StartScreenButtons : MonoBehaviour
{
    public UIDocument startScreenDocument;
    public StartGame startGame;
    public UISounds uiSounds;

    private void OnEnable()
    {
        var root = startScreenDocument.rootVisualElement;

        // Hook all buttons for sounds
        if (uiSounds != null)
        {
            uiSounds.HookAllButtons(root);
        }

        var startGameButton = root.Q<Button>("StartGame");
        if (startGameButton != null)
        {
            startGameButton.clicked += () =>
            {
                startGame.StartGameToMainMenu();
            };
        }

        var settingsButton = root.Q<Button>("Settings");
        if (settingsButton != null)
        {
            settingsButton.clicked += () =>
            {
                startGame.StartGameToSettings();
            };
        }

        var quitButton = root.Q<Button>("QuitGame");
        if (quitButton != null)
        {
            quitButton.clicked += () =>
            {
                startGame.OnQuitButtonPressed();
            };
        }
    }
}
