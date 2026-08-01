using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Linq;

public class Thorns : Trait
{
    private TabletController tablet;

    private int initialDamage;
    
    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "Deal 1 damage whenever you use a Block die";
        tablet.descText.text = description;

        // sceneStart = true;
        // roundStart = true;
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

        int newHealth = Opponent.Instance.currentHealth - damage;
        StartCoroutine(BattleSceneManager.Instance.AnimateOpponentHealthDecrease(newHealth, damage));
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
        int newHealth = Opponent.Instance.currentHealth - damage;
        StartCoroutine(BattleSceneManager.Instance.AnimateOpponentHealthDecrease(newHealth, damage));
    }

    public override void OnAcvitveCombatEnd()
    {
        Debug.Log("Triggered on AcvitveCombatEnd");
    }
}
