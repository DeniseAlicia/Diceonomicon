using System;
using UnityEngine;

public class Chaotic : Trait
{
    private BattleSceneManager battleSceneManager;
    private TabletController tablet;

    private DiceSlotData attackSlot;
    private DiceSlotData blockSlot;
    private DiceSlotData poisonSlot;
    private DiceSlotData mimicSlot;

    private DiceSlotData[] slots = new DiceSlotData[4];

    public void Start()
    {
        battleSceneManager = FindFirstObjectByType<BattleSceneManager>();
        tablet = GetComponent<TabletController>();

        attackSlot = Resources.Load<DiceSlotData>($"Slots/AttackSlot");
        blockSlot = Resources.Load<DiceSlotData>($"Slots/ShieldSlot");
        poisonSlot = Resources.Load<DiceSlotData>($"Slots/PoisonSlot");
        mimicSlot = Resources.Load<DiceSlotData>($"Slots/MimicrySlot");

        slots = new DiceSlotData[] { attackSlot, blockSlot, poisonSlot, mimicSlot };

        description = "Randomizes the slots every round.";
        tablet.descText.text = description;

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
        Main.ChangeSlotData(1, tablet, slots[(int)Math.Floor(UnityEngine.Random.Range(0f, 3.999f))]);
        Main.ChangeSlotData(2, tablet, slots[(int)Math.Floor(UnityEngine.Random.Range(0f, 2.999f))]);
        Main.ChangeSlotData(3, tablet, slots[(int)Math.Floor(UnityEngine.Random.Range(0f, 2.999f))]);
        Main.ChangeSlotData(4, tablet, slots[(int)Math.Floor(UnityEngine.Random.Range(0f, 3.999f))]);
        Main.ChangeSlotData(5, tablet, slots[(int)Math.Floor(UnityEngine.Random.Range(0f, 2.999f))]);
        Main.ChangeSlotData(6, tablet, slots[(int)Math.Floor(UnityEngine.Random.Range(0f, 2.999f))]);
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
