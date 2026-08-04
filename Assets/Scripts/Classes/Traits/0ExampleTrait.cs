public class ExampleTrait : Trait
{
    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "...";
        tablet.descText.text = description;

        BattleSceneManager.OnSceneStart.AddListener(OnSceneStart);
        BattleSceneManager.OnRoundStart.AddListener(OnRoundStart);
        BattleSceneManager.OnPlacementDone.AddListener(OnIntermissionDone);
        BattleSceneManager.OnAcvitveCombatStart.AddListener(OnAcvitveCombatStart);
        BattleSceneManager.OnAcvitveCombatEnd.AddListener(OnAcvitveCombatEnd);
        BattleSceneManager.OnSlotTriggered += OnSlotTriggered;
    }

    public void OnSceneStart() { }
    public void OnRoundStart() { }
    public void OnIntermissionDone() { }
    public void OnAcvitveCombatStart() { }
    public void OnAcvitveCombatEnd() { }
    public void OnSlotTriggered(DiceSlotController slot) { }

    public override void UnsubscribeFromEvents()
    {
        BattleSceneManager.OnSceneStart.RemoveListener(OnSceneStart);
        BattleSceneManager.OnRoundStart.RemoveListener(OnRoundStart);
        BattleSceneManager.OnPlacementDone.RemoveListener(OnIntermissionDone);
        BattleSceneManager.OnAcvitveCombatStart.RemoveListener(OnAcvitveCombatStart);
        BattleSceneManager.OnAcvitveCombatEnd.RemoveListener(OnAcvitveCombatEnd);
        BattleSceneManager.OnSlotTriggered -= OnSlotTriggered;
    }
}