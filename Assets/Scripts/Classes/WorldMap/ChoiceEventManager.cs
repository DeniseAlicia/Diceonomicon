using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class ChoiceEventManager : MonoBehaviour
{
    private GameStateManager gameState;
    private Waypoint waypoint;
    public List<Button> choices;

    private int level;
    private string area;
    private string type;
    private bool isRandom;
    private int amount;
    private int quality;

    private List<string> choiceList;
    public List<TMP_Text> nameTexts;
    public List<TMP_Text> descTexts;
    public List<TMP_Text> flavorTexts;
    public List<Image> images;
    public List<Texture> textures;
    public List<Image> emotions;
    public List<GameObject> backgroundObject;

    public Sprite placeholder;

    // RewardData
    public List<DiceData> diceRewards;
    public DiceData chosenDie;

    public List<TabletData> impRewards;
    public TabletData chosenImp;

    public List<CandleData> candleRewards;
    public CandleData chosenCandle;

    public List<RelicData> relicRewards;
    public RelicData chosenRelic;

    void Start()
    {
        gameState = FindFirstObjectByType<GameStateManager>();

        waypoint = GameStateManager.Instance.tempWp;
        level = gameState.player.level;
        area = gameState.player.area;
        type = gameState.player.type;

        isRandom = false;
        if (isRandom)
        {
            amount = 1;
        }
        else
        {
            amount = 3;
        }
        quality = level;

        SetChoices();
    }

    public void SetChoices()
    {
        switch (type)
        {
            case "Dice":
                DiceReward.GetRewards(type, quality, area, amount);
                choiceList = DiceReward.options;

                if (choiceList.Count < 3)
                {
                    Debug.LogError("Not enough rewards returned! Need 3, got " + choiceList.Count);
                    return;
                }

                for (int i = 0; i < amount; i++)
                {
                    string dataName = choiceList[i];
                    DiceData data = Resources.Load<DiceData>($"Dice/{dataName}");
                    nameTexts[i].text = data.title;
                    descTexts[i].text = data.desc;
                    flavorTexts[i].text = data.flavorText;

                    if (images[i].sprite != null)
                    {
                        images[i].sprite = data.image;
                    }

                    if (images[i].sprite == null)
                    {
                        images[i].sprite = placeholder;
                    }

                    string type = data.tags[0];
                    int emotionColor = Array.IndexOf(Main.diceTags, type);
                    emotions[i].color = Main.colors[emotionColor];
                    images[i].color = Main.colors[emotionColor];
                    backgroundObject[i].SetActive(true);

                    diceRewards.Add(data);
                }
                break;
            case "Impling":
                ImpReward.GetRewards(type, quality, area, amount);
                choiceList = ImpReward.options;

                if (choiceList.Count < 3)
                {
                    Debug.LogError("Not enough rewards returned! Need 3, got " + choiceList.Count);
                    return;
                }

                for (int i = 0; i < amount; i++)
                {
                    string dataName = choiceList[i];
                    TabletData data = Resources.Load<TabletData>($"Implings/{dataName}");
                    nameTexts[i].text = data.name;
                    descTexts[i].text = data.desc;
                    flavorTexts[i].text = data.trait;

                    if (textures[i] != null)
                    {
                        textures[i] = data.artwork;
                    }

                    if (textures[i] == null)
                    {
                        textures[i] = Resources.Load<Texture>($"Placeholder/ImpPlaceholder");
                    }

                    impRewards.Add(data);
                }
                break;
            case "Candlemaker":
                CandleReward.GetRewards();
                choiceList = CandleReward.options;

                if (choiceList.Count < 3)
                {
                    Debug.LogError("Not enough rewards returned! Need 3, got " + choiceList.Count);
                    return;
                }

                for (int i = 0; i < amount; i++)
                {
                    string dataName = choiceList[i];
                    CandleData data = Resources.Load<CandleData>($"Candles/{dataName}" + "CandleData");
                    nameTexts[i].text = data.title;
                    descTexts[i].text = data.desc;
                    flavorTexts[i].text = data.flavorText;

                    if (images[i].sprite != null)
                    {
                        images[i].sprite = data.image;
                    }

                    if (images[i].sprite == null)
                    {
                        images[i].sprite = placeholder;
                    }

                    candleRewards.Add(data);
                }
                break;
            case "Relic":
                RelicReward.GetRewards(type, quality, area, amount);
                choiceList = RelicReward.options;

                if (choiceList.Count < 3)
                {
                    Debug.LogError("Not enough rewards returned! Need 3, got " + choiceList.Count);
                    return;
                }

                for (int i = 0; i < amount; i++)
                {
                    string dataName = choiceList[i];
                    RelicData data = Resources.Load<RelicData>($"Relics/{dataName}");
                    nameTexts[i].text = data.title;
                    descTexts[i].text = data.desc;
                    flavorTexts[i].text = data.flavorText;

                    if (images[i].sprite != null)
                    {
                        images[i].sprite = data.image;
                    }

                    if (images[i].sprite == null)
                    {
                        images[i].sprite = placeholder;
                    }

                    relicRewards.Add(data);
                }
                break;
            default:
                break;
        }
    }

    public void AddChoice(Button button)
    {
        int i = 0;
        foreach (Button choice in choices)
        {
            if (button.name == choice.name)
            {
                switch (type)
                {
                    case "Dice":
                        DiceData die = diceRewards[i];
                        GameStateManager.Instance.player.diceDeck.Add(die);
                        //Debug.Log(die.name + " added");
                        break;
                    case "Impling":
                        break;
                    case "Candlemaker":
                        CandleData candle = candleRewards[i];
                        candle.DoEffect();
                        //Debug.Log("Candle effect!");
                        break;
                    case "Relic":
                        RelicData relic = relicRewards[i];
                        GameStateManager.Instance.player.relics.Add(relic);
                        //Debug.Log("Added Relic!");
                        break;
                    default:
                        break;
                }
            }
            i++;
        }
        //waypoint.SpawnCluster();
        SceneManager.UnloadSceneAsync("RewardSelection");
    }

}
