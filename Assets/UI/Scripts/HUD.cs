using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    private VisualElement root;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        var backButton = root.Q<Button>("BackToMainMenu");
        if (backButton != null)
        {
            backButton.clicked += OnBackButtonClicked;
        }
    }

    private void OnBackButtonClicked()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
