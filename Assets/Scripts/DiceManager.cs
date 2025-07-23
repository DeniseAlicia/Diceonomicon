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
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                RollAllDice();
            }

            bool allDiceSleeping = false;
            int i = 0;
            foreach (Die die in dice)
            {
                if (die.stoop == true)
                {
                    i++;
                    Debug.Log(i);
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
            foreach (Die die in dice)
            {
                // die.ResetDiePosition();
                die.RollDice();
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

                // rotate die to a "straight" position
                // float rotationX = dice[i].transform.eulerAngles.x;
                // float rotationZ = dice[i].transform.eulerAngles.z;
                // dice[i].transform.eulerAngles = new Vector3(rotationX, 0f, rotationZ);

                dice[i].stoop = false;
            }
        }
    }
}

