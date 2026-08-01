using System;
using UnityEngine;

public class Rotator : Trait
{
    private TabletController tablet;
    private float[] angle = { -90, 90 };

    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "You can rotate your tablets one additional time per round";
        tablet.descText.text = description;

        OnSceneStart();
        // roundStart = true;
        // acvitveCombatStart = true;
        // placementDone = true;
        acvitveCombatEnd = true;

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
        int i = (int)Math.Round(UnityEngine.Random.Range(0f, 1f));
        tablet.Rotate(angle[i]);
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
        int i = (int)UnityEngine.Random.Range(0f, 1f);
        tablet.Rotate(angle[i]);
    }
}
