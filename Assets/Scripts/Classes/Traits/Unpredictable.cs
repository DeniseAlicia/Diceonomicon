using System;
using UnityEngine;

public class Unpredictable : Trait
{
    private Unpredictable Instance;

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
        Instance.tablet = GetComponent<TabletController>();

        attackSlot = Resources.Load<DiceSlotData>($"Slots/AttackSlot");
        blockSlot = Resources.Load<DiceSlotData>($"Slots/ShieldSlot");
        poisonSlot = Resources.Load<DiceSlotData>($"Slots/PoisonSlot");

        slots = new DiceSlotData[] { attackSlot, blockSlot, poisonSlot };

        description = "Randomizes the slots every round.";
        tablet.descText.text = description;

        Instance.OnSceneStart();
        BattleSceneManager.OnRoundStart.AddListener(OnRoundStart);
    }


    public void OnSceneStart()
    {
        Instance.attacks = 0;
        Instance.blocks = 0;
        Instance.poisons = 0;

        for (int i = 1; i - 1 < Instance.tablet.tabletSlots.Count; i++)
        {
            int randomNumber = (int)Math.Floor(UnityEngine.Random.Range(0f, 2.999f));
            SlotAction.ChangeSlotData(i, tablet, slots[ValidateRandom(randomNumber)]);
        }
    }

    public void OnRoundStart()
    {
        Instance.attacks = 0;
        Instance.blocks = 0;
        Instance.poisons = 0;

        for (int i = 1; i - 1 < Instance.tablet.tabletSlots.Count; i++)
        {
            int randomNumber = (int)Math.Floor(UnityEngine.Random.Range(0f, 2.999f));
            SlotAction.ChangeSlotData(i, tablet, slots[ValidateRandom(randomNumber)]);
        }
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

    public override void UnsubscribeFromEvents()
    {
        BattleSceneManager.OnSceneStart.RemoveListener(OnSceneStart);
        BattleSceneManager.OnRoundStart.RemoveListener(OnRoundStart);
    }
}