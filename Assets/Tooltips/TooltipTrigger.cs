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

    public void Update()
    {
        if (controller == null || !controller.HasSlotData() || Input.GetAxis("Mouse X") > 0.5 || Input.GetAxis("Mouse Y") > 0.5)
        {
            TooltipSystem.HideTooltip();
        }
    }

    // mouse enters element
    public void OnMouseEnter()
    {
        isPointerOver = true;
        if (tooltipCoroutine != null)
        {
            StopCoroutine(tooltipCoroutine);
        }
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

