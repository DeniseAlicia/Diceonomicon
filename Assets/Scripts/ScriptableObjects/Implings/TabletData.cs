using UnityEngine;

[CreateAssetMenu(fileName = "TabletData", menuName = "Data/TabletData")]
public class TabletData : CollectableData
{
    public bool enemy = false;
    public bool unlocked = false;

    public new string name;
    public string desc;
    public Texture artwork;
    public Sprite uiArtwork;
    public Entity owner;
    public int health;
    public DiceSlotData[] slots;
    public DiceData[] startingDice;
    public GameObject tabletPrefab;
    public GameObject slotPrefab;
    public string trait;

    private Vector3 startSlotPosition = new Vector3(-3, 0, -3);

    public void CreateSlots(Transform tabletTransform, TabletController tablet)
    {
        Vector3 currentSlotPosition = startSlotPosition;

        for (int i = 0; i < slots.Length; i++)
        {
            if (enemy == false)
            {
                owner = FindFirstObjectByType<Player>();
            }
            else
            {
                owner = FindFirstObjectByType<Opponent>();
            }

            string slotName = $"Slot{i + 1}";
            Transform targetSlot = tabletTransform.Find(slotName);

            GameObject diceSlotInstance = Instantiate(slotPrefab, targetSlot);
            diceSlotInstance.transform.localPosition = Vector3.zero;

            diceSlotInstance.name = $"SlotInstance{i + 1}";

            DiceSlotController controller = diceSlotInstance.GetComponent<DiceSlotController>();
            controller.owner = owner;
            controller.SetData(slots[i]);

            tablet.tabletSlots.Add(controller);

            TooltipTrigger tooltipTrigger = diceSlotInstance.GetComponent<TooltipTrigger>();
            if (tooltipTrigger == null)
            {
                tooltipTrigger = diceSlotInstance.AddComponent<TooltipTrigger>();
            }
        }
    }
}
