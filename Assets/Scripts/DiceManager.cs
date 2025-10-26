using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using DG.Tweening;


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

    public static void SortAllDice(List<Die> dice, BattleSceneManager sceneManager, Button button)
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
            //dice[i].transform.position = diePos;

            overflow = Mathf.Floor(i / 3);
            spacing = (i - overflow * 3) * distance;

            diePos.x += spacing;
            diePos.z += overflow * distance;
            //dice[i].transform.position = diePos;

            dice[i].transform.DOMove(diePos, 0.2f).SetEase(Ease.OutQuad);
        }

        foreach (Die die in sceneManager.player.dice)
        {
            die.isResting = false;
        }

        button.gameObject.SetActive(true);
    }

    static void RestartOnClick()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void CopyDice(Entity entity)
    {
        List<Die> intermissionDice = new List<Die>();

        foreach (Die die in entity.extraDice)
        {
            if (die == null) continue;


            Die dieCopy = UnityEngine.Object.Instantiate(die, die.transform.position, Quaternion.identity);
            dieCopy.textureRenderer.material.SetTexture("_BaseMap", dieCopy.data.texture);
            dieCopy.InitializeAsCopy();

            intermissionDice.Add(dieCopy);
        }

        float distance = 0.5f;
        Vector3 basePos = new Vector3(-0.5f, 0.15f, -1f);

        for (int i = 0; i < intermissionDice.Count; i++)
        {
            float overflow = Mathf.Floor(i / 3);
            float spacing = (i - overflow * 3) * distance;

            Vector3 diePos = basePos;
            diePos.x += spacing;
            diePos.z += overflow * distance;

            intermissionDice[i].transform.position = diePos;
        }

        entity.tempDice = intermissionDice;
    }
}


