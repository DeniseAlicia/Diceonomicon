using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    public List<CollectableData> rewards;
    public CollectableData reward;

    void Start()
    {
        gameState = FindFirstObjectByType<GameStateManager>();

        waypoint = GameStateManager.Instance.tempWp;
        level = gameState.player.level;
        area = gameState.player.area;
        // type = gameState.player.type;
        type = "Dice";
        isRandom = false;
        if (isRandom)
        {
            amount = 1;
        }
        else
        {
            amount = 3;
        }
        quality = 0;

        SetChoices();
    }

    public void SetChoices()
    {

        Rewards.GetRewards(type, quality, area, amount);
        choiceList = Rewards.options;

        if (choiceList.Count < 3)
        {
            Debug.LogError("Not enough rewards returned! Need 3, got " + choiceList.Count);
            return;
        }

        switch (type)
        {
            case "Dice":
                for (int i = 0; i < amount; i++)
                {
                    string dataName = choiceList[i];
                    DiceData data = Resources.Load<DiceData>($"Dice/{dataName}");
                    nameTexts[i].text = data.title;
                    descTexts[i].text = data.desc;
                    flavorTexts[i].text = data.flavorText;
                    images[i].sprite = data.image;
                    rewards.Add(data);
                }
                break;
            case "Impling":
                break;
            default:
                break;
        }
    }

    public void AddChoice(Button button)
    {
        foreach (Button choice in choices)
        {
            int i = 0;
            if (button.name == choice.name)
            {
                switch (type)
                {
                    case "Dice":
                        DiceData die = (DiceData)rewards[i];
                        GameStateManager.Instance.player.diceDeck.Add(die);
                        //Debug.Log(die.name + " added");
                        break;
                    case "Impling":
                        break;
                    default:
                        break;
                }
            }
            i++;
        }
        waypoint.SpawnCluster();
        SceneManager.UnloadSceneAsync("RewardSelection");
    }

}
