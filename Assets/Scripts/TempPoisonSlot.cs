// using UnityEngine;

// public class TempPoisonSlot : MonoBehaviour
// {
//     public BattleSceneManager battleSceneManager;

//     public void DoEffect()
//     {
//         if (0 == 0) // check for owner of diceSlot
//         {
//             for (int i = 0; i < battleSceneManager.playerActiveColumn.slottedDie.value; i++)
//             {
//                 int rdm = Random.Range(0, battleSceneManager.enemyActiveColumn.Count + 1);
//                 Die frozenDie = battleSceneManager.enemyActiveColumn[rdm];

//                 if (frozenDie != null)
//                 {
//                     frozenDie.value -= 1;
//                 }
//             }
//         }
//         else
//         {
//             for (int i = 0; i < battleSceneManager.enemyActiveColumn.slottedDie.value; i++)
//             {
//                 int rdm = Random.Range(0, battleSceneManager.playerActiveColumn.Count + 1);
//                 Die frozenDie = battleSceneManager.playerActiveColumn[rdm];

//                 if (frozenDie != null)
//                 {
//                     frozenDie.value -= 1;
//                 }
//             }
//         }
//     }
// }
