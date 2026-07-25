using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

public class LoadCanvas : MonoBehaviour
{
    public static LoadCanvas Instance;

    [Header("Scene Objects")]
    public GameObject[] banners;
    public GameObject score;
    public GameObject chest;
    public GameObject[] candles;
    public GameObject[] health;
    public GameObject buttons;
    public GameObject textbubble;
    public GameObject book;
    public TMP_Text text;
    public BattleSceneManager sceneManager;

    [Header("Animation Settings")]
    [SerializeField] private Vector3 topEntryOffset = new Vector3(0f, 50f, 0f);
    [SerializeField] private Vector3 bottomEntryOffset = new Vector3(0f, -50f, 0f);
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float staggerDelay = 0.2f;

    private bool textActive;
    private float delay;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        delay = 20f;
        AnimateObjectsIn();
        CycleReminderText();
    }

    private void AnimateObjectsIn()
    {
        AnimateGroup(banners, staggerDelay, topEntryOffset, false);
        AnimateSingle(score, 0.3f, topEntryOffset, false);

        AnimateGroup(candles, staggerDelay * 0f, bottomEntryOffset * 2, false);
        AnimateGroup(health, staggerDelay * 0.3f, bottomEntryOffset, false);
        AnimateSingle(chest, 0.5f, bottomEntryOffset, false);
        AnimateSingle(buttons, 0.6f, bottomEntryOffset, false);
        AnimateSingle(book, 0.8f, bottomEntryOffset, false);
    }


    public void AnimateObjectsOut()
    {
        AnimateGroup(banners, staggerDelay, topEntryOffset, true);
        AnimateSingle(score, 0.3f, topEntryOffset, true);

        AnimateGroup(candles, staggerDelay * 0f, bottomEntryOffset * 2, true);
        AnimateGroup(health, staggerDelay * 0.3f, bottomEntryOffset, true);
        AnimateSingle(chest, 0.5f, bottomEntryOffset, true);
        AnimateSingle(buttons, 0.6f, bottomEntryOffset, true);
        AnimateSingle(book, 0.8f, bottomEntryOffset, true);
    }

    private void AnimateGroup(GameObject[] objects, float delayStep, Vector3 entryOffset, bool outro)
    {
        Vector3 startPos;
        Vector3 endPos;

        for (int i = 0; i < objects.Length; i++)
        {
            var obj = objects[i];
            if (obj == null) continue;

            Transform t = obj.transform;

            if (!outro)
            {
                startPos = t.localPosition + entryOffset;
                endPos = t.localPosition;
            }
            else
            {
                startPos = t.localPosition;
                endPos = t.localPosition + entryOffset;
            }

            t.localPosition = startPos;
            obj.SetActive(true);

            t.DOLocalMove(endPos, moveDuration)
             .SetEase(Ease.OutQuad)
             .SetDelay(i * delayStep);
        }
    }

    private void AnimateSingle(GameObject obj, float delay, Vector3 entryOffset, bool outro)
    {
        Vector3 startPos;
        Vector3 endPos;

        if (obj == null) return;

        Transform t = obj.transform;
        if (!outro)
        {
            startPos = t.localPosition + entryOffset;
            endPos = t.localPosition;
        }
        else
        {
            startPos = t.localPosition;
            endPos = t.localPosition + entryOffset;
        }

        t.localPosition = startPos;
        obj.SetActive(true);

        t.DOLocalMove(endPos, moveDuration)
         .SetEase(Ease.OutQuad)
         .SetDelay(delay);
    }

    private void CycleReminderText()
    {
        if (sceneManager.round < 3)
        {
            StartCoroutine(StartReminderText());
        }
        else
        {
            textbubble.SetActive(false);
        }
    }

    public IEnumerator StartReminderText()
    {
        yield return new WaitForSeconds(delay);
        if (textActive)
        {
            delay = 30f;
            textActive = false;
            textbubble.SetActive(false);
            CycleReminderText();
        }
        else
        {
            delay = 20f;
            textActive = true;
            text.text = GetReminderText();
            textbubble.SetActive(true);
            CycleReminderText();
        }
    }

    private string GetReminderText()
    {
        string[] reminders = { "You can rotate green dice by pressing the Right-Mouse Button while you drag them.", "When you place two or more dice in a column directly below each other and they share a slot color, their value is multiplied by the length of the uninterrupted chain.", "Each time a Spell slot triggers it targets a random slot in the opponent's column and applies the spell's effect on it.", "Dice trigger in the following order: \n Buff → Spell → Damage/Block" };

        System.Random rand = new System.Random();

        int index = rand.Next(reminders.Length);

        string randomReminder = reminders[index];

        return randomReminder;
    }
}

