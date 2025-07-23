using UnityEngine;
using UnityEngine.UIElements;

public class UISounds : MonoBehaviour
{
    public FMODUnity.EventReference fmodEvent;

    [SerializeField]
    private string[] excludedButtonNames = new string[] { "CloseMenu", "Button_to_LevelOne" };


    private VisualElement root;

    public void HookAllButtons(VisualElement rootElement)
    {
        if (rootElement == null)
        {
            Debug.LogError("rootElement is null!");
            return;
        }

        // find all buttons 
        var buttons = rootElement.Query<Button>().ToList();
        Debug.Log($"Buttons found: {buttons.Count}");

        foreach (var btn in buttons)
        {
            if (IsExcluded(btn.name))
                continue;

            btn.clicked += () =>
            {
                Debug.Log("Button clicked");
                if (!fmodEvent.IsNull)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(fmodEvent.Path, transform.position);
                }
                else
                {
                    Debug.LogWarning("FMOD Event Reference is null!");
                }
            };
        }
    }

    private bool IsExcluded(string buttonName)
    {
        foreach (var excluded in excludedButtonNames)
        {
            if (buttonName.Equals(excluded, System.StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }
}
