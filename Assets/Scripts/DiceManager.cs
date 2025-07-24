namespace Diceonomicon
{

    using System;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class DiceManager : MonoBehaviour
    {
        [SerializeField] Die[] dice;
        private Camera camGameplay;
        private Camera camBattleTablets;

        void Start()
        {
            camGameplay = GameObject.Find("Gameplay").GetComponent<Camera>();
            camBattleTablets = GameObject.Find("BattleTablets").GetComponent<Camera>();
        }

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
            foreach (Die die in dice)
            {
                die.ResetDiePosition();
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

                dice[i].isResting = false;
            }
        }
    }
}

