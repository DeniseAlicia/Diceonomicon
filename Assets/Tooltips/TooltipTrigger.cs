using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;

    [Multiline()] public string content;

    private Coroutine tooltipCoroutine;
    private bool isPointerOver = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        Debug.Log("Trigger");
        tooltipCoroutine = StartCoroutine(ShowTooltipWithDelay());
    }

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

        if (isPointerOver)
        {
            TooltipSystem.ShowTooltip(content, header);
        }
    }
}

