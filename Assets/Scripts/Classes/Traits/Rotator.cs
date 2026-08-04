using System;

public class Rotator : Trait
{
    private float[] angle = { -90, 90 };

    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "You can rotate your tablets one additional time per round";
        tablet.descText.text = description;

        OnSceneStart();
        BattleSceneManager.OnAcvitveCombatEnd.AddListener(OnAcvitveCombatEnd);
    }

    public void OnSceneStart()
    {
        int i = (int)Math.Round(UnityEngine.Random.Range(0f, 1f));
        tablet.Rotate(angle[i]);
    }

    public void OnAcvitveCombatEnd()
    {
        int i = (int)UnityEngine.Random.Range(0f, 1f);
        tablet.Rotate(angle[i]);
    }

    public override void UnsubscribeFromEvents()
    {
        BattleSceneManager.OnSceneStart.RemoveListener(OnSceneStart);
        BattleSceneManager.OnAcvitveCombatEnd.RemoveListener(OnAcvitveCombatEnd);
    }
}