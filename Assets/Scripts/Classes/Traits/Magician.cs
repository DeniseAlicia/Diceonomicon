using UnityEngine;

public class Magician : Trait
{
    private BattleSceneManager battleSceneManager;
    private TabletController tablet;

    public void Start()
    {
        battleSceneManager = FindFirstObjectByType<BattleSceneManager>();
        tablet = GetComponent<TabletController>();

        description = "Spell dice gain a +1 bonus and Damage dice get a -1 penalty";
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
        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.tag == "Spell")
            {
                if (slot.slottedDie != null && slot.slottedDie.isCopy)
                {
                    slot.slottedDie.value += 1;
                    slot.slottedDie.TranslateValue();
                }
            }
        }

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.tag == "Damage")
            {
                if (slot.slottedDie != null && slot.slottedDie.isCopy)
                {
                    slot.slottedDie.value -= 1;
                    slot.slottedDie.TranslateValue();
                }
            }
        }
    }

    public override void OnAcvitveCombatStart()
    {
        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.tag == "Spell")
            {
                if (slot.slottedDie != null)
                {
                    slot.slottedDie.value += 1;
                    slot.slottedDie.TranslateValue();
                }
            }
        }

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.tag == "Damage")
            {
                if (slot.slottedDie != null)
                {
                    slot.slottedDie.value -= 1;
                    slot.slottedDie.TranslateValue();
                }
            }
        }
    }

    public override void OnAcvitveCombatEnd()
    {
        Debug.Log("Triggered on AcvitveCombatEnd");
    }
}
