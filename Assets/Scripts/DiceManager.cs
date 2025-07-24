using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DiceManager : MonoBehaviour
{
    public List<DiceData> diceDrawn;

    private Vector3 startPosition = new Vector3(0f, 0f, 0f);
    public GameObject diePrefab;

    void Start()
    {
        Vector3 currentPosition = startPosition;

        foreach (DiceData die in diceDrawn)
        {
            GameObject dieInstance = Instantiate(diePrefab, currentPosition, Quaternion.identity);

            DiceController controller = dieInstance.GetComponent<DiceController>();
            controller.SetData(die);
            currentPosition.y += 0.5f;


        }
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Test");
            RollAllDice();
        }
    }

    private void RollAllDice()
    {
        Die[] dice = GameObject.FindObjectsByType<Die>(FindObjectsSortMode.None);

        foreach (Die die in dice)
        {
            die.Roll();
        }
    }
}
