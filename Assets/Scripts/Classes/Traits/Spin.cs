public class Spin : Trait
{
    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "You can rotate your tablets one additional time per round";
        tablet.descText.text = description;

        OnSceneStart();
    }

    public void OnSceneStart()
    {
        tablet.maxRotations += 1;
    }

    public override void UnsubscribeFromEvents()
    {
        BattleSceneManager.OnSceneStart.RemoveListener(OnSceneStart);
    }
}