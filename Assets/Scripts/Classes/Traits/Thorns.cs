using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Linq;

public class Thorns : Trait
{
    private BattleSceneManager battleSceneManager;
    private TabletController tablet;

    private int initialDamage;
    
    public void Start()
    {
        battleSceneManager = FindFirstObjectByType<BattleSceneManager>();
        tablet = GetComponent<TabletController>();

        // sceneStart = true;
        // roundStart = true;
        acvitveCombatStart = true;
        placementDone = true;
        // acvitveCombatEnd = true;

        if (sceneStart)
        {
            battleSceneManager.OnSceneStart.AddListener(OnSceneStart);
        }
        if (roundStart)
        {
            battleSceneManager.OnRoundStart.AddListener(OnRoundStart);
        }
        if (placementDone)
        {
            battleSceneManager.OnPlacementDone.AddListener(OnPlacementDone);
        }
        if (acvitveCombatStart)
        {
            battleSceneManager.OnAcvitveCombatStart.AddListener(OnAcvitveCombatStart);
        }
        if (acvitveCombatEnd)
        {
            battleSceneManager.OnAcvitveCombatEnd.AddListener(OnAcvitveCombatEnd);
        }

        Debug.Log("Starting...");
    }


    public override void OnSceneStart()
    {
        Debug.Log("Triggered on SceneStart");
    }

    public override void OnRoundStart()
    {
        Debug.Log("Triggered on RoundStart");
    }

    public override void OnPlacementDone()
    {
        int damage = 0;

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slottedDie != null && slot.slottedDie.dieTags.Contains("Block"))
            {
                damage++;
            }
        }

        damage -= initialDamage;
        initialDamage = damage;

        int newHealth = battleSceneManager.opponent.currentHealth - damage;
        StartCoroutine(battleSceneManager.AnimateOpponentHealthDecrease(newHealth, damage));
    }

    public override void OnAcvitveCombatStart()
    {
        int damage = 0;

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slottedDie != null && slot.slottedDie.dieTags.Contains("Block"))
            {
                damage++;
            }
        }

        initialDamage = damage;
        int newHealth = battleSceneManager.opponent.currentHealth - damage;
        StartCoroutine(battleSceneManager.AnimateOpponentHealthDecrease(newHealth, damage));
    }

    public override void OnAcvitveCombatEnd()
    {
        Debug.Log("Triggered on AcvitveCombatEnd");
    }
}
