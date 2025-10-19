using UnityEngine;

public class Recycle : Trait
{
    private BattleSceneManager battleSceneManager;
    private TabletController tablet;

    public void Start()
    {
        battleSceneManager = FindFirstObjectByType<BattleSceneManager>();
        tablet = GetComponent<TabletController>();

        description = "Heal by 1 for every unused die";
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
        int healing = 0;

        foreach (Die die in battleSceneManager.player.tempDice)
        {
            if (!die.isPlaced)
            {
                healing++;
            }
        }

        int newHealth = battleSceneManager.player.currentHealth + healing;
        StartCoroutine(battleSceneManager.AnimatePlayerHealthIncrease(newHealth, healing));
    }

    public override void OnAcvitveCombatStart()
    {
        int healing = battleSceneManager.unusedDice.Count;
        int newHealth = battleSceneManager.player.currentHealth + healing;
        StartCoroutine(battleSceneManager.AnimatePlayerHealthIncrease(newHealth, healing));
    }

    public override void OnAcvitveCombatEnd()
    {
        Debug.Log("Triggered on AcvitveCombatEnd");
    }
}
