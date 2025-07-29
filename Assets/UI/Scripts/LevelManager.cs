using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class LevelManager : MonoBehaviour
{
    public void SetupLevelButtons(VisualElement mapMenuRoot)
    {
        var levelOneButton = mapMenuRoot.Q<Button>("Button_to_LevelOne");

        if (levelOneButton != null)
        {
            levelOneButton.clicked += () =>
            {
                SceneManager.LoadScene("LevelOne");
            };
        }
        else
        {
            Debug.LogWarning("Level Button not found");
        }
    }
}
