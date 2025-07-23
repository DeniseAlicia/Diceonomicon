using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private DiceSlotController controller;
    private Coroutine tooltipCoroutine;
    private bool isPointerOver = false;


    private void Awake()
    {
        controller = GetComponent<DiceSlotController>();
    }


    // mouse enters element
    public void OnPointerEnter(PointerEventData eventData)
    {
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
        float delay = 0.5f;
        float elapsed = 0f;

        while (elapsed < delay)
        {
            if (!isPointerOver) yield break;

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (isPointerOver && controller != null && controller.HasSlotData())
        {
            TooltipSystem.ShowTooltip(
                controller.GetTooltipDescription(),
                controller.GetTooltipHeader()
            );
        }
    }
}

