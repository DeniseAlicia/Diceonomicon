using UnityEngine;

public class Adaptive : Trait
{
    private BattleSceneManager battleSceneManager;
    private TabletController tablet;

    public void Start()
    {
        battleSceneManager = FindFirstObjectByType<BattleSceneManager>();
        tablet = GetComponent<TabletController>();
        
        description = "Damage dice gain a +1 bonus and Block dice get a -1 penalty";
        tablet.descText.text = description;

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
        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.tag == "Block")
            {
                if (slot.slottedDie != null && slot.slottedDie.isCopy)
                {
                    if (battleSceneManager.player.currentHealth < battleSceneManager.player.maxHealth / 2)
                    {
                        slot.slottedDie.value += 1;
                        slot.slottedDie.TranslateValue();
                    }
                    else
                    {
                        slot.slottedDie.value -= 1;
                        slot.slottedDie.TranslateValue();
                    }
                }
            }
        }

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.tag == "Damage")
            {
                if (slot.slottedDie != null && slot.slottedDie.isCopy)
                {
                    if (battleSceneManager.player.currentHealth < battleSceneManager.player.maxHealth / 2)
                    {
                        slot.slottedDie.value -= 1;
                        slot.slottedDie.TranslateValue();
                    }
                    else
                    {
                        slot.slottedDie.value += 1;
                        slot.slottedDie.TranslateValue();
                    }
                }
            }
        }
    }

    public override void OnAcvitveCombatStart()
    {
        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.tag == "Block")
            {
                if (slot.slottedDie != null)
                {
                    if (battleSceneManager.player.currentHealth < battleSceneManager.player.maxHealth / 2)
                    {
                        slot.slottedDie.value += 1;
                        slot.slottedDie.TranslateValue();
                    }
                    else
                    {
                        slot.slottedDie.value -= 1;
                        slot.slottedDie.TranslateValue();
                    }
                }
            }
        }

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.tag == "Damage")
            {
                if (slot.slottedDie != null)
                {
                    if (battleSceneManager.player.currentHealth < battleSceneManager.player.maxHealth / 2)
                    {
                        slot.slottedDie.value -= 1;
                        slot.slottedDie.TranslateValue();
                    }
                    else
                    {
                        slot.slottedDie.value += 1;
                        slot.slottedDie.TranslateValue();
                    }
                }
            }
        }
    }

    public override void OnAcvitveCombatEnd()
    {
        Debug.Log("Triggered on AcvitveCombatEnd");
    }
}
