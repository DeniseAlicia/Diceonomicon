using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DiceManager : MonoBehaviour
{
    public List<DiceData> diceDrawn;
    public Die[] dice;

    private Vector3 startPosition = new Vector3(0f, 0f, 0f);
    public GameObject diePrefab;
    private Camera camGameplay;
    private Camera camBattleTablets;

    void Start()
    {
        Vector3 currentPosition = startPosition;

        foreach (DiceData die in diceDrawn)
        {
            GameObject dieInstance = Instantiate(diePrefab, currentPosition, Quaternion.identity);

            DiceController controller = dieInstance.GetComponent<DiceController>();
            controller.SetData(die);
            currentPosition.y += 0.5f;

            camGameplay = GameObject.Find("Gameplay").GetComponent<Camera>();
            camBattleTablets = GameObject.Find("BattleTablets").GetComponent<Camera>();
        }
    }

    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Debug.Log("Test");
            RollAllDice();
        }

        bool allDiceSleeping = false;
            int i = 0;
            foreach (Die die in dice)
            {
                if (die.isResting == true)
                {
                    i++;
                    // Debug.Log(i);
                }

            }

            if (i == dice.Length)
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
            Die[] dice = GameObject.FindObjectsByType<Die>(FindObjectsSortMode.None);
            foreach (Die die in dice)
            {
                die.ResetDiePosition();
                die.Roll();
            }
        }

        private void SortAllDice()
        {
            float overflow = 0;
            float spacing;
            float distance = 0.5f;
            Vector3 diePos;

            for (int i = 0; i < dice.Length; i++)
            {
                diePos = new Vector3(-1f, 0.1f, -1f);
                dice[i].transform.position = diePos;

                overflow = Mathf.Floor(i / 3);
                spacing = (i - overflow * 3) * distance;

                diePos.x += spacing;
                diePos.z += overflow * distance;
                dice[i].transform.position = diePos;

                dice[i].isResting = false;
            }
        }
    }


