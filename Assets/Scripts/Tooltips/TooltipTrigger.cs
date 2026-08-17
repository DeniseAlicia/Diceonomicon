using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour
{
    private DiceSlotController slot;
    private Die die;
    private Coroutine tooltipCoroutine;
    private bool isPointerOver = false;
    private bool isDie = false;


    private void Start()
    {
        die = GetComponent<Die>();
        if (die != null)
        {
            isDie = true;
        }

        if (isDie)
        {
            if (die.parentSlot != null)
            {
                slot = die.parentSlot;
            }
        }
        else
        {
            slot = GetComponent<DiceSlotController>();
            if (slot != null && slot.slottedDie != null)
            {
                die = slot.slottedDie;
            }
        }
    }

    public void OnMouseEnter()
    {
        if (die != null)
        {
            if (!die.isBeingDragged)
            {
                isPointerOver = true;

                if (tooltipCoroutine != null)
                {
                    StopCoroutine(tooltipCoroutine);
                }
                tooltipCoroutine = StartCoroutine(ShowTooltipWithDelay());

            }
            else
            {
                return;
            }
        }
        else
        {
            isPointerOver = true;
            if (tooltipCoroutine != null)
            {
                StopCoroutine(tooltipCoroutine);
            }
            tooltipCoroutine = StartCoroutine(ShowTooltipWithDelay());
        }
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

        if (isDie)
        {
            if (isPointerOver && die != null && die.HasDieData())
            {
                if (die.parentSlot != null && die.parentSlot.HasSlotData() && !die.isRolling)
                {
                    slot = die.parentSlot;

                    TooltipSystem.ShowSlotTooltip(
                        slot.GetTooltipDescription(),
                        slot.GetTooltipHeader());
                }

                TooltipSystem.ShowDieTooltip(
                    die.GetTooltipDescription(),
                    die.GetTooltipHeader());
            }
        }

        if (isPointerOver && slot != null && slot.HasSlotData())
        {
            TooltipSystem.ShowSlotTooltip(
                         slot.GetTooltipDescription(),
                         slot.GetTooltipHeader());
        }


        TooltipSystem.UpdateTooltip();
        tooltipCoroutine = null;
    }
}

