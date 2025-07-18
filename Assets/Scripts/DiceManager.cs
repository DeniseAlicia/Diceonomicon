namespace Diceonomicon
{
    using System;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class DiceManager : MonoBehaviour
    {
        [SerializeField] Die[] dice;

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
                die.RollDice();
            }
        }
    }
}
