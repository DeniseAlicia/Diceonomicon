using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    public GameObject tutorialText;

    public void Toggle()
    {
        tutorialText.SetActive(!tutorialText.activeSelf);
    }
}
