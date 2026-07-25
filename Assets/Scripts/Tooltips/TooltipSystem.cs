using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem Instance;

    public Tooltip tooltip;
    public Tooltip tooltipSlot;
    public Tooltip tooltipDie;

    public void Awake()
    {
        Instance = this;
    }

    public static void ShowSlotTooltip(string content, string header = "")
    {
        Instance.tooltipSlot.SetText(content, header);
        if (string.IsNullOrEmpty(header) && string.IsNullOrEmpty(content))
        {
            return;
        }
        Instance.tooltipSlot.gameObject.SetActive(true);
    }


    public static void ShowDieTooltip(string content, string header = "")
    {
        Instance.tooltipDie.SetText(content, header);
        if (string.IsNullOrEmpty(header) && string.IsNullOrEmpty(content))
        {
            return;
        }
        Instance.tooltipDie.gameObject.SetActive(true);
    }

    public static void HideTooltip()
    {
        Instance.tooltipSlot.gameObject.SetActive(false);
        Instance.tooltipDie.gameObject.SetActive(false);
    }

    public static void UpdateTooltip()
    {
        Instance.tooltip.UpdatePosition();
        // current.tooltipSlot.UpdatePosition();
        // current.tooltipDie.UpdatePosition();
    }


}
