using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Linq;

public class Lifedrink : Trait
{
    private BattleSceneManager battleSceneManager;
    private TabletController tablet;

    private int initialHealing;

    public void Start()
    {
        battleSceneManager = FindFirstObjectByType<BattleSceneManager>();
        tablet = GetComponent<TabletController>();

        description = "Heal by 1 whenever you use a Damage die";
        tablet.descText.text = description;

        // sceneStart = true;
        //roundStart = true;
        acvitveCombatStart = true;
        placementDone = true;
        // acvitveCombatEnd = true;

        if (sceneStart)
        {
            BattleSceneManager.OnSceneStart.AddListener(OnSceneStart);
        }
        if (roundStart)
        {
            BattleSceneManager.OnRoundStart.AddListener(OnRoundStart);
        }
        if (placementDone)
        {
            BattleSceneManager.OnPlacementDone.AddListener(OnPlacementDone);
        }
        if (acvitveCombatStart)
        {
            BattleSceneManager.OnAcvitveCombatStart.AddListener(OnAcvitveCombatStart);
        }
        if (acvitveCombatEnd)
        {
            BattleSceneManager.OnAcvitveCombatEnd.AddListener(OnAcvitveCombatEnd);
        }
    }


    public override void OnSceneStart()
    {
        Debug.Log("Triggered on SceneStart");
    }

    public override void OnRoundStart()
    {
        
    }

    public override void OnPlacementDone()
    {
        int healing = 0;

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slottedDie != null && slot.slottedDie.dieTags.Contains("Damage"))
            {
                healing++;
            }
        }

        healing -= initialHealing;
        initialHealing = healing;

        int newHealth = battleSceneManager.player.currentHealth + healing;
        StartCoroutine(battleSceneManager.AnimatePlayerHealthIncrease(newHealth, healing));
    }

    public override void OnAcvitveCombatStart()
    {
        initialHealing = 0;
        int healing = 0;

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slottedDie != null && slot.slottedDie.dieTags.Contains("Damage"))
            {
                healing++;
            }
        }

        initialHealing = healing;
        int newHealth = battleSceneManager.player.currentHealth + healing;
        StartCoroutine(battleSceneManager.AnimatePlayerHealthIncrease(newHealth, healing));
    }

    public override void OnAcvitveCombatEnd()
    {
        Debug.Log("Triggered on AcvitveCombatEnd");
    }
}
