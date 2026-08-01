using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] public DiceData dieData;
    [SerializeField] private Image[] icons;
    [SerializeField] private TMP_Text[] sides;

    public void SetChoicePreview()
    {
        GameStateManager gameState = FindFirstObjectByType<GameStateManager>();

        if (gameState.player.type != "Dice")
        {
            return;
        }

        foreach (Image icon in icons)
        {
            string type = dieData.tags[0];
            int emotionColor = Array.IndexOf(Emotions.types, type);
            icon.color = Emotions.colors[emotionColor];
            icon.sprite = dieData.image;
        }

        for (int i = 0; i < sides.Length && i < dieData.range.Length; i++)
        {
            if (!dieData.tags.Contains("Buff"))
            {
                sides[i].text = dieData.range[i].ToString();
            }
            else if (dieData.tags.Contains("Buff"))
            {
                List<int> directions = new List<int>();

                string valueString = dieData.range[i].ToString();
                foreach (char x in valueString)
                {
                    int newValue = Int32.Parse(x.ToString());
                    int angle = newValue * 45 + 45;
                    directions.Add(angle);
                }
                sides[i].text = "";

                foreach (int angle in directions)
                {
                    TextMeshProUGUI arrow = new GameObject("Arrow").AddComponent<TextMeshProUGUI>();

                    arrow.transform.SetParent(sides[i].transform, false);
                    arrow.text = "◄";
                    arrow.fontSize = 14;

                    if (directions.Count() < 2)
                    {
                        arrow.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 16);
                        arrow.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 16);
                    }
                    else if (directions.Count() < 3)
                    {
                        arrow.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 26);
                        arrow.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 26);
                    }
                    else if (directions.Count() > 3)
                    {
                        arrow.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 32);
                        arrow.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 32);
                    }
                    arrow.alignment = TextAlignmentOptions.MidlineJustified;
                    arrow.rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
                    arrow.rectTransform.localPosition = Vector3.zero;
                }

            }
        }
    }
}
