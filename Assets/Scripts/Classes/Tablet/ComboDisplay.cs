using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class ComboDisplay : MonoBehaviour
{
    public TMP_Text comboText;
    public GameObject comboDisplayObject;
    public SpriteRenderer backgroundSprite;
    [SerializeField] private DiceSlotController slot;

    public int combo;

    public void SetComboColor()
    {
        if (slot.slottedDie != null &&  !slot.slottedDie.statuses.Contains(Status.Inactive))
        {
            int emotionColor = Array.IndexOf(Emotions.types, slot.slottedDie.dieTags[0]);
            backgroundSprite.color = Emotions.colors[emotionColor];
        }
    }

    public void UpdateComboText()
    {
        combo = slot.comboSlots.Count() + 1 + slot.tempMult;
        ShowComboDisplay();
        comboText.text = combo.ToString();
        comboText.fontSize = 28 + (2 * combo);
    }

    public void ShowComboDisplay()
    {
        if (slot.comboSlots.Count() > 0 && slot.slottedDie != null && slot.slotTag != "Buff" && !slot.slottedDie.statuses.Contains(Status.Inactive))
        {
            comboDisplayObject.SetActive(true);
        }
        else
        {
            comboDisplayObject.SetActive(false);
        }
    }

    public void UpdateCombo()
    {
        slot.comboSlots.Clear();
        slot.DetectComboDown(slot.transform.position);
        slot.DetectComboUp(slot.transform.position);
        combo = slot.comboSlots.Count() + 1 + slot.tempMult;
        UpdateComboText();
        SetComboColor();
    }
}
