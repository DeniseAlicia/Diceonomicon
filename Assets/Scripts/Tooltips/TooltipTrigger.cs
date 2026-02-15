using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour
{
    private DiceSlotController controller;
    private Coroutine tooltipCoroutine;
    private bool isPointerOver = false;


    private void Start()
    {
        controller = GetComponent<DiceSlotController>();
    }

    public void OnMouseEnter()
    {
        isPointerOver = true;
        if (tooltipCoroutine != null)
        {
            StopCoroutine(tooltipCoroutine);
        }
        tooltipCoroutine = StartCoroutine(ShowTooltipWithDelay());
    }

    public void OnMouseExit()
    {
        isPointerOver = false;
        TooltipSystem.HideTooltip();
    }

    private IEnumerator ShowTooltipWithDelay()
    {
        float delay = 0.01f;
        yield return new WaitForSeconds(delay);

        if (isPointerOver && controller != null && controller.HasSlotData())
        {
            TooltipSystem.ShowTooltip(
                controller.GetTooltipDescription(),
                controller.GetTooltipHeader()
            );
        }
        tooltipCoroutine = null;
    }
}

