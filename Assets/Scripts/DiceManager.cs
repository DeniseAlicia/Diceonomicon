using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DiceManager : MonoBehaviour
{
    [SerializeField] List<Die> dice;

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
        foreach (Die die in dice)
        {
            //die.ResetDiePosition();
            die.Roll();
        }
    }
}
