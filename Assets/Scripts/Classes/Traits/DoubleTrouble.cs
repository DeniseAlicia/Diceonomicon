using UnityEngine;

public class DoubleTrouble : Trait
{
    private BattleSceneManager battleSceneManager;
    private TabletController tablet;

    private bool isFilled;
    private bool isInEffect;

    public void Start()
    {
        battleSceneManager = FindFirstObjectByType<BattleSceneManager>();
        tablet = GetComponent<TabletController>();

        description = "Whena every slot is filled, double all the dice values";
        tablet.descText.text = description;

        // sceneStart = true;
        roundStart = true;
        acvitveCombatStart = true;
        placementDone = true;
        //acvitveCombatEnd = true;

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

    public void Update()
    {
        if (isFilled && !isInEffect)
        {
            foreach (DiceSlotController slot in tablet.tabletSlots)
            {
                slot.slottedDie.value = slot.slottedDie.value + slot.slottedDie.value;
                slot.slottedDie.TranslateValue();
            }

            isInEffect = true;
        }

        if (!isFilled)
        {
            isInEffect = false;
        }
    }

    public override void OnSceneStart()
    {
        Debug.Log("Triggered on SceneStart");
    }

    public override void OnRoundStart()
    {
        int slotsFilled = 0;

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slottedDie != null)
            {
                slotsFilled += 1;
            }
        }

        if (slotsFilled == tablet.tabletSlots.Count)
        {
            isFilled = true;
        }
        else
        {
            isFilled = false;
        }
    }

    public override void OnPlacementDone()
    {
        int slotsFilled = 0;

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slottedDie != null)
            {
                slotsFilled += 1;
            }
        }

        if (slotsFilled == tablet.tabletSlots.Count)
        {
            isFilled = true;
            Debug.Log("Filled");
        }
        else
        {
            isFilled = false;
            Debug.Log("Not filled");
        }
    }

    public override void OnAcvitveCombatStart()
    {
        int slotsFilled = 0;

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (slot.slottedDie != null)
            {
                slotsFilled += 1;
            }
        }

        if (slotsFilled == tablet.tabletSlots.Count)
        {
            isFilled = true;
        }
        else
        {
            isFilled = false;
        }
    }

    public override void OnAcvitveCombatEnd()
    {

    }
}
