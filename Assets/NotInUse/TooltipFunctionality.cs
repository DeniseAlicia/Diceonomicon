using UnityEngine;
using UnityEngine.UIElements;

//displays the Tooltip in the UI (box with text)
public class UITooltip : MonoBehaviour
{
    //access the Tooltip UXML and the Tooltip CSS
    public VisualTreeAsset tooltipDocument;
    public StyleSheet tooltipStyle;


    private VisualElement tooltipContainer;
    private Label tooltipText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(VisualElement root)
    {
        //create a tooltip out of the template
        tooltipContainer = tooltipDocument.CloneTree();

        //apply the style sheet
        tooltipContainer.styleSheets.Add(tooltipStyle);

        //get the text field from the tooltip UXML
        tooltipText = tooltipContainer.Q<Label>("TooltipText");

        //hide tooltip at start without text
        tooltipText.text = "";
        tooltipContainer.style.display = DisplayStyle.None;

        //add the tooltip to the UI (in the UI Document)
        root.Add(tooltipContainer);

        //register mousemove event
        root.RegisterCallback<MouseMoveEvent>(OnMouseMove);
    }

    private void OnMouseMove(MouseMoveEvent evt)
    {
        //position the tooltip offset from the mouse pointer (10px to the right/bottom)
        tooltipContainer.style.left = evt.mousePosition.x + 10;
        tooltipContainer.style.top = evt.mousePosition.y + 10;
    }

    public void ShowTooltip(string text)
    {
        tooltipText.text = text;
        tooltipContainer.style.display = DisplayStyle.Flex;
    }

    public void HideTooltip()
    {
        tooltipContainer.style.display = DisplayStyle.None;
    }
}
