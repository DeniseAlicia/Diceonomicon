public class Traitless : Trait
{
    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "I don't have any special abilities";
        tablet.descText.text = description;
    }

    public override void UnsubscribeFromEvents() { }
}