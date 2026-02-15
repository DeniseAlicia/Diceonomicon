using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class UISounds : MonoBehaviour
{
    public FMODUnity.EventReference fmodEvent;
    public FMODUnity.EventReference hoverSound;
    public FMODUnity.EventReference specialClickSound;

    [SerializeField]
    private string[] excludedButtonNames = new string[] { "CloseMenu", "Button_to_LevelOne" };

    private Dictionary<string, float> lastHoverTime = new Dictionary<string, float>();
    private float hoverCooldown = 0.5f;
    private VisualElement root;

    public void HookAllButtons(VisualElement rootElement)
    {
        if (rootElement == null)
        {
            Debug.LogError("rootElement is null!");
            return;
        }

        var buttons = rootElement.Query<Button>().ToList();

        foreach (var btn in buttons)
        {
            if (IsExcluded(btn.name))
            {
                // special Click-Sound
                btn.clicked += () =>
                {

                    if (!specialClickSound.IsNull)
                    {
                        var instance = FMODUnity.RuntimeManager.CreateInstance(specialClickSound);
                        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
                        instance.start();
                        instance.release();
                    }
                };
            }
            else
            {
                // normale Click-Sound
                btn.clicked += () =>
                {
                    if (!fmodEvent.IsNull)
                    {
                        var instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
                        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
                        instance.start();
                        instance.release();
                    }
                };
            }

            // Hover-Sound 
            btn.RegisterCallback<MouseEnterEvent>((evt) =>
            {
                float currentTime = Time.time;
                if (!lastHoverTime.TryGetValue(btn.name, out float lastTime) || currentTime - lastTime > hoverCooldown)
                {
                    if (!hoverSound.IsNull)
                    {
                        var instance = FMODUnity.RuntimeManager.CreateInstance(hoverSound);
                        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
                        instance.start();
                        instance.release();
                    }

                    lastHoverTime[btn.name] = currentTime;
                }
            });
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
