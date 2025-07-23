using UnityEngine;
using UnityEngine.UIElements;

public class UISounds : MonoBehaviour
{
    public FMODUnity.EventReference fmodEvent;

    [SerializeField]
    private string[] excludedButtonNames = new string[] { "CloseMenu", "Button_to_LevelOne" };


    private VisualElement root;

    public void HookAllButtons()
    {
        // get UIDocument 
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument Component not found!");
            return;
        }

        root = uiDocument.rootVisualElement;

        // find all buttons 
        var buttons = root.Query<Button>().ToList();

        foreach (var btn in buttons)
        {
            if (IsExcluded(btn.name))
                continue;

            btn.clicked += () =>
            {
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
