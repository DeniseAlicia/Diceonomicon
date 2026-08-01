using UnityEngine;

public class Recycle : Trait
{
    private TabletController tablet;

    public void Start()
    {
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

        foreach (Die die in Player.Instance.tempDice)
        {
            if (!die.isPlaced)
            {
                healing++;
            }
        }

        int newHealth = Player.Instance.currentHealth + healing;
        StartCoroutine(BattleSceneManager.Instance.AnimatePlayerHealthIncrease(newHealth, healing));
    }

    public override void OnAcvitveCombatStart()
    {
        int healing = BattleSceneManager.Instance.unusedDice.Count;
        int newHealth = Player.Instance.currentHealth + healing;
        StartCoroutine(BattleSceneManager.Instance.AnimatePlayerHealthIncrease(newHealth, healing));
    }

    public override void OnAcvitveCombatEnd()
    {
        Debug.Log("Triggered on AcvitveCombatEnd");
    }
}
