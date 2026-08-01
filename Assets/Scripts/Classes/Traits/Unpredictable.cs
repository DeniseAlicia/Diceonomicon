using System;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UIElements;

public class Unpredictable : Trait
{
    private Unpredictable Instance;

    private BattleSceneManager battleSceneManager;
    private TabletController tablet;

    private DiceSlotData attackSlot;
    private DiceSlotData blockSlot;
    private DiceSlotData poisonSlot;

    private int attacks;
    private int blocks;
    private int poisons;

    private DiceSlotData[] slots = new DiceSlotData[3];

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        battleSceneManager = FindFirstObjectByType<BattleSceneManager>();
        Instance.tablet = GetComponent<TabletController>();

        attackSlot = Resources.Load<DiceSlotData>($"Slots/AttackSlot");
        blockSlot = Resources.Load<DiceSlotData>($"Slots/ShieldSlot");
        poisonSlot = Resources.Load<DiceSlotData>($"Slots/PoisonSlot");

        slots = new DiceSlotData[] { attackSlot, blockSlot , poisonSlot};

        description = "Randomizes the slots every round.";
        tablet.descText.text = description;

        Instance.OnSceneStart();
        Instance.roundStart = true;
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
        Instance.attacks = 0;
        Instance.blocks = 0;
        Instance.poisons = 0;

        for (int i = 1; i - 1 < Instance.tablet.tabletSlots.Count; i++)
        {
            int randomNumber = (int)Math.Floor(UnityEngine.Random.Range(0f, 2.999f));
            Main.ChangeSlotData(i, tablet, slots[ValidateRandom(randomNumber)]);
        }
    }

    public override void OnRoundStart()
    {
        Instance.attacks = 0;
        Instance.blocks = 0;
        Instance.poisons = 0;

        for (int i = 1; i - 1 < Instance.tablet.tabletSlots.Count; i++)
        {
            int randomNumber = (int)Math.Floor(UnityEngine.Random.Range(0f, 2.999f));
            Main.ChangeSlotData(i, tablet, slots[ValidateRandom(randomNumber)]);
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


    public int ValidateRandom(int randomNumber)
    {
        switch (randomNumber)
        {
            case 0:
                if (Instance.attacks < 3)
                {
                    Instance.attacks++;
                    return randomNumber;
                }
                goto case 1;
            case 1:
                if (Instance.blocks < 3)
                {
                    Instance.blocks++;
                    randomNumber = 1;
                    return randomNumber;
                }
                goto case 2;
            case 2:
                if (Instance.poisons < 3)
                {
                    Instance.poisons++;
                    randomNumber = 2;
                    return randomNumber;
                }
                goto case default;
            default:
                randomNumber = 0;
                return randomNumber;
        }
    }
}