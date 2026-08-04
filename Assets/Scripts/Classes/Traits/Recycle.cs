public class Recycle : Trait
{
    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "Heal by 1 for every unused die";
        tablet.descText.text = description;

        BattleSceneManager.OnPlacementDone.AddListener(OnPlacementDone);
        BattleSceneManager.OnAcvitveCombatStart.AddListener(OnAcvitveCombatStart);
    }

    public void OnPlacementDone()
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

    public void OnAcvitveCombatStart()
    {
        int healing = BattleSceneManager.Instance.unusedDice.Count;
        int newHealth = Player.Instance.currentHealth + healing;
        StartCoroutine(BattleSceneManager.Instance.AnimatePlayerHealthIncrease(newHealth, healing));
    }

    public override void UnsubscribeFromEvents()
    {
        BattleSceneManager.OnPlacementDone.RemoveListener(OnPlacementDone);
        BattleSceneManager.OnAcvitveCombatStart.RemoveListener(OnAcvitveCombatStart);
    }
}