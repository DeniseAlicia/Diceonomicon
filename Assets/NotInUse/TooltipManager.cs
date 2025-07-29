using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//connects the buttons with the Tooltip-Texts and reacts to mouse events
public class TooltipManager : MonoBehaviour
{
    //reference to UITooltip script
    public UITooltip tooltip;

    private Dictionary<string, string> tooltipTexts = new Dictionary<string, string>()
    {
        { "button_to_Main", "Return to main menu." },
        { "button_to_Settings", "Open settings." },
        { "button_to_Map", "Show level map." },
        { "button_to_Dice", "Show dice collection." },
        { "button_to_Implings", "Open Impling compendium." },
        { "CloseMenu", "Close menu." },
        { "LevelMap", "Open the full level overview." }
    };

    public void RegisterTooltip(Button button)
    {
        if (button == null || tooltip == null)
            return;

        if (tooltipTexts.TryGetValue(button.name, out string tooltipText))
        {
            button.userData = tooltipText;
        }
        else
        {
            button.userData = null;
        }

        button.RegisterCallback<MouseEnterEvent>(evt =>
        {
            string tooltipText = button.userData as string;
            if (!string.IsNullOrEmpty(tooltipText))
            {
                tooltip.ShowTooltip(tooltipText);
            }
        });

        button.RegisterCallback<MouseLeaveEvent>(evt =>
        {
            tooltip.HideTooltip();
        });
    }

    public void RegisterTooltipsForMenu(VisualElement menu)
    {
        var buttons = menu.Query<Button>().ToList();

        foreach (var button in buttons)
        {
            RegisterTooltip(button);
        }
    }
}
