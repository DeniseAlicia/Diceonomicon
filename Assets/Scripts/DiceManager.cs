using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiceManager : MonoBehaviour
{
    [SerializeField] Die[] _dice;

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RollAllDice();
        }

    }

    private void RollAllDice()
    {
        foreach (Die die in _dice)
        {
            die.ResetDiePosition();
            die.RollDice();
        }
    }
}
