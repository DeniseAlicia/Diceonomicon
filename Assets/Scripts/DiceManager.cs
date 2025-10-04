using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class DiceManager
{
    public static List<DiceData> diceDrawn;
    public static List<Die> dice;

    private static DiceTrayWall[] diceTrayWalls;
    private static Vector3 startPosition = new Vector3(0f, 5f, -5f);
    public static GameObject diePrefab;
    private static Camera camGameplay;
    private static Camera camBattleTablets;

    public static void Start()
    {
        camGameplay = GameObject.Find("Gameplay").GetComponent<Camera>();
        camBattleTablets = GameObject.Find("BattleTablets").GetComponent<Camera>();

        diceTrayWalls = GameObject.FindObjectsByType<DiceTrayWall>(FindObjectsSortMode.None);
    }

    private static void RollAllDice()
    {
        foreach (Die die in dice)
        {
            die.Roll(0.5f);
        }
    }

    public static void SortAllDice(List<Die> dice, BattleSceneManager sceneManager)
    {
        foreach (Die die in sceneManager.player.dice)
        {
            die.GetSideFacingUp();
            die.isResting = true;
            die.isDraggable = true;
            die.rigidBody.isKinematic = true;
            die.rigidBody.useGravity = false;
        }

        float overflow = 0;
        float spacing;
        float distance = 0.5f;
        Vector3 diePos;

        for (int i = 0; i < dice.Count; i++)
        {
            diePos = new Vector3(-0.5f, 0.15f, -1f);
            dice[i].transform.position = diePos;

            overflow = Mathf.Floor(i / 3);
            spacing = (i - overflow * 3) * distance;

            diePos.x += spacing;
            diePos.z += overflow * distance;
            dice[i].transform.position = diePos;

            dice[i].isResting = false;
        }
    }

    static void RestartOnClick()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


