using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DiceManager : MonoBehaviour
{
    public List<DiceData> diceDrawn;
    public List<Die> dice;

    private Vector3 startPosition = new Vector3(0f, 5f, -5f);
    public GameObject diePrefab;
    private Camera camGameplay;
    private Camera camBattleTablets;

    public Button drawButton;
    public Button rollButton;
    public Button confirmButton;
    public Button restartButton;

    void Start()
    {
        camGameplay = GameObject.Find("Gameplay").GetComponent<Camera>();
        camBattleTablets = GameObject.Find("BattleTablets").GetComponent<Camera>();

        Button draw = drawButton.GetComponent<Button>();
        draw.onClick.AddListener(ResetAllDice);

        Button roll = rollButton.GetComponent<Button>();
        roll.onClick.AddListener(RollAllDice);

        Button confirm = confirmButton.GetComponent<Button>();
        //confirm.onClick.AddListener(ConfirmOnClick);

        Button restart = restartButton.GetComponent<Button>();
        restart.onClick.AddListener(RestartOnClick);

        Vector3 basePosition = startPosition;
        float distance = 0.5f;


        for (int i = 0; i < diceDrawn.Count; i++)
        {
            DiceData die = diceDrawn[i];

            float overflow = Mathf.Floor(i / 3f);
            float spacing = (i - overflow * 3) * distance;

            Vector3 diePos = basePosition;
            diePos.x += spacing;
            diePos.z += overflow * distance;

            // Instantiate prefab at the calculated position
            GameObject dieInstance = Instantiate(diePrefab, diePos, Quaternion.identity);

            // Set data on the die script
            Die controller = dieInstance.GetComponent<Die>();
            controller.SetData(die);

            // Add to list
            dice.Add(controller);
        }
    }

    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Debug.Log("Test");
            ResetAllDice();
            RollAllDice();
        }

        bool allDiceSleeping = false;
        int i = 0;
        foreach (Die die in dice)
        {
            if (die.isResting == true)
            {
                i++;
            }
        }

        if (i == dice.Count)
        {
            allDiceSleeping = true;
        }

        if (allDiceSleeping == true)
        {
            SortAllDice();
        }
    }

    private void RollAllDice()
    {
        foreach (Die die in dice)
        {
            die.Roll();
        }
    }

    private void ResetAllDice()
    {
        foreach (Die die in dice)
        {
            die.ResetDiePosition();
        }
    }

    private void SortAllDice()
    {
        float overflow = 0;
        float spacing;
        float distance = 0.5f;
        Vector3 diePos;

        for (int i = 0; i < dice.Count; i++)
        {
            diePos = new Vector3(-0.5f, 0.5f, -1f);
            dice[i].transform.position = diePos;

            overflow = Mathf.Floor(i / 3);
            spacing = (i - overflow * 3) * distance;

            diePos.x += spacing;
            diePos.z += overflow * distance;
            dice[i].transform.position = diePos;

            dice[i].isResting = false;
        }
    }

    void RestartOnClick()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


