using System.Linq;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public static class DieAction
{
    public static void RangeToValue(Die die)
    {
        if (!die.dieTags.Contains("Buff") && !die.statuses.Contains(Status.Inactive))
        {
            foreach (Transform childSide in die.GetDiceSides())
            {
                GameObject child = childSide.gameObject;

                if (!int.TryParse(child.name, out int index))
                {
                    continue;
                }

                int translatedValue = die.range[index - 1];
                GameObject childText = child.transform.GetChild(0).gameObject;
                childText.GetComponent<TMP_Text>().text = translatedValue.ToString();
            }
        }
        else if (die.dieTags.Contains("Buff"))
        {
            ShowBuffArrows(die);
            if (die.value > 0 && die.value < 7)
            {
                die.value = die.range[die.value - 1];
            }
        }
    }

    public static void ValueToAngle(Die die, List<int> directions)
    {
        directions.Clear();
        string valueString = die.value.ToString();
        foreach (char x in valueString)
        {
            int newValue = Int32.Parse(x.ToString());
            int angle = newValue * 45 - 45 + die.dieRotation;
            directions.Add(angle);
        }
    }

    public static void ShowBuffArrows(Die die)
    {
        for (int i = 0; i < die.GetDiceSides().Length && i < die.data.range.Length; i++)
        {
            List<int> directions = new List<int>();

            string valueString = die.range[i].ToString();
            foreach (char x in valueString)
            {
                int angle = 0;
                int newValue = Int32.Parse(x.ToString());

                if (newValue % 2 == 0)
                {
                    angle = newValue * 45 + 45 - 90;
                }
                else
                {
                    angle = newValue * 45 + 45;
                }

                directions.Add(angle);
            }

            foreach (int angle in directions)
            {
                GameObject textObj = new GameObject("Arrow");

                textObj.transform.SetParent(die.GetDiceSides()[i].GetChild(0).transform);
                textObj.transform.localPosition = Vector3.zero;
                textObj.transform.localRotation = Quaternion.Euler(0, 0, 0);

                TextMeshPro tmp = textObj.AddComponent<TextMeshPro>();
                tmp.text = " ◄";
                tmp.fontSize = 5;
                tmp.alignment = TextAlignmentOptions.Center;
                tmp.alignment = TextAlignmentOptions.Midline;
                tmp.transform.localRotation = Quaternion.Euler(0, 0, angle);
                tmp.transform.localScale = new Vector3(1, 1, 1);
                textObj.transform.localPosition = Vector3.zero;
                tmp.font = Resources.Load<TMP_FontAsset>("Fonts/BuffFont");

                switch (directions.Count)
                {
                    case 1:
                        tmp.font = Resources.Load<TMP_FontAsset>("Fonts/BuffFont1");
                        break;

                    case 2:
                        tmp.font = Resources.Load<TMP_FontAsset>("Fonts/BuffFont2");
                        break;

                    case 3:
                        tmp.font = Resources.Load<TMP_FontAsset>("Fonts/BuffFont4");
                        break;

                    default:
                        tmp.font = Resources.Load<TMP_FontAsset>("Fonts/BuffFont4");
                        break;
                }
            }
        }
        die.MoveToLayer("Gameplay");
    }


    public static void UpdateText(Die die)
    {
        GameObject child = die.sideUp.gameObject;
        GameObject childText = child.transform.GetChild(0).gameObject;
        childText.GetComponent<TMP_Text>().text = die.value.ToString();

        if (die.originalValue == die.value)
        {
            return;
        }

        if (die.originalValue < die.value)
        {
            childText.GetComponent<TMP_Text>().color = Color.gold;
            return;
        }

        if (die.originalValue > die.value)
        {
            childText.GetComponent<TMP_Text>().color = Color.purple;
        }
    }
}
