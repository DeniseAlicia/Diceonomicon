using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Overload : MonoBehaviour
{
    public bool isOverloading;
    public bool isOverloaded;
    public TabletController tablet;
    public TMP_Text overloadText;

    public void Start()
    {
        BattleSceneManager.OnRoundStart.AddListener(OnRoundStart);
        BattleSceneManager.OnAcvitveCombatStart.AddListener(OnAcvitveCombatStart);
    }

    public void OnRoundStart()
    {
        DecayEmotions(tablet);
        UpdateEmotions(tablet);
        //EndOverload(tablet);
    }

    public void OnAcvitveCombatStart()
    {
        AddEmotion(tablet.tabletSlots);
    }

    public void AddEmotion(List<DiceSlotController> tabletSlots)
    {
        foreach (DiceSlotController slot in tabletSlots)
        {
            if (slot.isFilled)
            {
                TabletController tablet = slot.GetComponentInParent<TabletController>();
                Die die = slot.GetComponentInChildren<Die>();
                foreach (string tag in die.dieTags)
                {
                    int index = System.Array.IndexOf(Main.diceTags, tag);

                    if (index >= 0)
                    {
                        tablet.emotionValues[index] += 1;
                    }
                }
            }
        }
    }

    public void DecayEmotions(TabletController tablet)
    {
        for (int i = 0; i < tablet.emotionValues.Length; i++)
        {
            if (tablet.emotionValues[i] > 0)
            {
                tablet.emotionValues[i] -= 1;
            }
        }
    }

    public void UpdateEmotions(TabletController tablet)
    {
        if (tablet.owner.GetType() == typeof(Player))
        {
            for (int i = 0; i < tablet.emotionValues.Length; i++)
            {
                int valueOfOtherEmotions = 0;

                for (int j = 0; j < tablet.emotionValues.Length; j++)
                {
                    if (j == i) continue;

                    valueOfOtherEmotions += tablet.emotionValues[j];
                }

                if (tablet.emotionValues[i] >= 5 && tablet.emotionValues[i] > valueOfOtherEmotions)
                {
                    {
                        if (isOverloading)
                        {
                            HandleOverload(tablet, i);
                        }
                        else
                        {
                            isOverloading = true;
                        }
                    }
                }
            }
        }
    }

    public void HandleOverload(TabletController tablet, int emotionIndex)
    {
        isOverloading = false;
        isOverloaded = true;
        overloadText.color = Main.colors[emotionIndex];

        switch (emotionIndex)
        {
            case 0: // Anger
                overloadText.text = "Raging";
                break;
            case 1: // Sadness
                overloadText.text = "Weeping";
                break;
            case 2: // Fear
                overloadText.text = "Terrified";
                break;
            case 3: // Envy
                overloadText.text = "Resentful";
                break;
            case 4: // Indifference
                overloadText.text = "Detached";
                break;
            case 5: // Temptation
                overloadText.text = "Stunned";
                break;
        }
    }

    public void EndOverload(TabletController tablet)
    {
        overloadText.text = "";
        overloadText.color = Color.white;
        isOverloaded = false;
    }
}
