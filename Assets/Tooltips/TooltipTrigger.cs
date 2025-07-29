using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerExitHandler
{
    private DiceSlotController controller;
    private Coroutine tooltipCoroutine;
    private bool isPointerOver = false;


    private void Start()
    {
        controller = GetComponent<DiceSlotController>();
    }


    // mouse enters element
    public void OnMouseEnter()
    {
        Debug.Log("Tooltip");
        isPointerOver = true;
        tooltipCoroutine = StartCoroutine(ShowTooltipWithDelay());
    }

    // mouse exits element
    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;

        if (tooltipCoroutine != null)
        {
            StopCoroutine(tooltipCoroutine);
            tooltipCoroutine = null;
        }

        TooltipSystem.HideTooltip();
    }

    private IEnumerator ShowTooltipWithDelay()
    {
        float delay = 1f;
        yield return new WaitForSeconds(delay);

        if (isPointerOver && controller != null && controller.HasSlotData())
        {
            TooltipSystem.ShowTooltip(
                controller.GetTooltipDescription(),
                controller.GetTooltipHeader()
            );
        }
    }
}

