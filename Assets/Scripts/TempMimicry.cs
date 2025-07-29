// using System;
// using UnityEngine;
// using UnityEngine.InputSystem;
// using System.Collections.Generic;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;

// public class TempMimicry : MonoBehaviour
// {
//     public BattleSceneManager battleSceneManager;
//     // public List<Die> mimicryDice = new List<Die>();

//     public List<Die> DoEffect()
//     {
//         List<Die> mimicryDice = new List<Die>();

//         if (0 == 0) // check for owner of diceSlot
//         {
//             for (int i = 0; i < battleSceneManager.playerActiveColumn.slottedDie.value; i++)
//             {
//                 int rdm = Random.Range(0, battleSceneManager.enemyActiveColumn.Count + 1);
//                 Die mimicDie = battleSceneManager.enemyActiveColumn[rdm];

//                 if (mimicDie != null)
//                 {
//                     mimicryDice.Add(battleSceneManager.enemyActiveColumn[rdm]);
//                 }
//             }
//         }
//         else
//         {
//             for (int i = 0; i < battleSceneManager.enemyActiveColumn.slottedDie.value; i++)
//             {
//                 int rdm = Random.Range(0, battleSceneManager.playerActiveColumn.Count + 1);
//                 Die mimicDie = battleSceneManager.playerActiveColumn[rdm];

//                 if (mimicDie != null)
//                 {
//                     mimicryDice.Add(battleSceneManager.playerActiveColumn[rdm]);
//                 }
//             }
//         }
//         return mimicryDice;
//     }
// }
