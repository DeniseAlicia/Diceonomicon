using UnityEngine;

public class Spin : Trait
{
    private BattleSceneManager battleSceneManager;


    public void Start()
    {
        battleSceneManager = FindFirstObjectByType<BattleSceneManager>();

        sceneStart = true;
        // roundStart = true;
        // acvitveCombatStart = true;
        // placementDone = true;
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

        Debug.Log("Starting...");
    }


    public override void OnSceneStart()
    {
        RotationButton.maxRotations += 1;
    }

    public override void OnRoundStart()
    {
        Debug.Log("Triggered on RoundStart");
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
