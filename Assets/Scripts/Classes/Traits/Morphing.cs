using UnityEngine;

public class Morphing : Trait
{
    private BattleSceneManager battleSceneManager;
    private TabletController tablet;
    private bool aggressive;

    private DiceSlotData attackSlot;
    private DiceSlotData blockSlot;

    public void Start()
    {
        battleSceneManager = FindFirstObjectByType<BattleSceneManager>();
        tablet = GetComponent<TabletController>();

        description = "Swaps Attack and Shield slot positions every round.";
        tablet.descText.text = description;

        attackSlot = Resources.Load<DiceSlotData>($"Slots/AttackSlot");
        blockSlot = Resources.Load<DiceSlotData>($"Slots/ShieldSlot");

        // sceneStart = true;
        roundStart = true;
        // acvitveCombatStart = true;
        // placementDone = true;
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

        Debug.Log("Starting...");
    }


    public override void OnSceneStart()
    {
        Debug.Log("Triggered on SceneStart");
    }

    public override void OnRoundStart()
    {
        if (aggressive)
        {
            SlotAction.ChangeSlotData(1, tablet, attackSlot);
            SlotAction.ChangeSlotData(2, tablet, attackSlot);
            SlotAction.ChangeSlotData(4, tablet, blockSlot);
            SlotAction.ChangeSlotData(5, tablet, blockSlot);
            SlotAction.ChangeSlotData(6, tablet, attackSlot);
            SlotAction.ChangeSlotData(8, tablet, blockSlot);
            SlotAction.ChangeSlotData(9, tablet, attackSlot);
            aggressive = false;
        }
        else
        {
            SlotAction.ChangeSlotData(1, tablet, blockSlot);
            SlotAction.ChangeSlotData(2, tablet, blockSlot);
            SlotAction.ChangeSlotData(4, tablet, attackSlot);
            SlotAction.ChangeSlotData(5, tablet, attackSlot);
            SlotAction.ChangeSlotData(6, tablet, blockSlot);
            SlotAction.ChangeSlotData(8, tablet, attackSlot);
            SlotAction.ChangeSlotData(9, tablet, blockSlot);
            aggressive = true;
        }
    }

    public override void OnPlacementDone()
    {
        Debug.Log("Triggered on PlacementDone");
    }

    public override void OnAcvitveCombatStart()
    {
        Debug.Log("Triggered on AcvitveCombatStart");
    }

    public override void OnAcvitveCombatEnd()
    {
        Debug.Log("Triggered on AcvitveCombatEnd");
    }
}
