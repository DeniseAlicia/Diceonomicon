using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Masked : Trait
{
    private List<DiceSlotController> slots;

    public void Start()
    {
        tablet = GetComponent<TabletController>();
        slots = tablet.tabletSlots;

        description = "Hides the values of dice";
        tablet.descText.text = description;
    }

    public void Update()
    {
        foreach (DiceSlotController slot in slots)
        {
            if (slot.isFilled && slot.slottedDie != null && slot.tag != "Buff")
            {
                foreach (Transform childSide in slot.slottedDie.GetDiceSides())
                {
                    GameObject child = childSide.gameObject;

                    if (!int.TryParse(child.name, out int index))
                    {
                        continue;
                    }

                    GameObject childText = child.transform.GetChild(0).gameObject;
                    childText.GetComponent<TMP_Text>().text = "?";
                }
            }
        }
    }

    public override void UnsubscribeFromEvents() { }
}