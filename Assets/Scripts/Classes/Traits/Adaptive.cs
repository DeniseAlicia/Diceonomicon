using UnityEngine;

public class Adaptive : Trait
{
    private TabletController tablet;

    public void Start()
    {
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
            if (slot.tag == "Block")
            {
                if (slot.slottedDie != null && slot.slottedDie.isCopy)
                {
                    if (Player.Instance.currentHealth < Player.Instance.maxHealth / 2)
                    {
                        slot.slottedDie.value += 1;
                        DieAction.UpdateText(slot.slottedDie);
                    }
                    else
                    {
                        slot.slottedDie.value -= 1;
                        DieAction.UpdateText(slot.slottedDie);
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
                    if (Player.Instance.currentHealth < Player.Instance.maxHealth / 2)
                    {
                        slot.slottedDie.value -= 1;
                        DieAction.UpdateText(slot.slottedDie);
                    }
                    else
                    {
                        slot.slottedDie.value += 1;
                        DieAction.UpdateText(slot.slottedDie);
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
                    if (Player.Instance.currentHealth < Player.Instance.maxHealth / 2)
                    {
                        slot.slottedDie.value += 1;
                        DieAction.UpdateText(slot.slottedDie);
                    }
                    else
                    {
                        slot.slottedDie.value -= 1;
                        DieAction.UpdateText(slot.slottedDie);
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
                    if (Player.Instance.currentHealth < Player.Instance.maxHealth / 2)
                    {
                        slot.slottedDie.value -= 1;
                        DieAction.UpdateText(slot.slottedDie);
                    }
                    else
                    {
                        slot.slottedDie.value += 1;
                        DieAction.UpdateText(slot.slottedDie);
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
