using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TabletData", menuName = "Data/TabletData")]
public class TabletData : ScriptableObject
{
    public bool enemy = false;

    public new string name;
    public string desc;
    public Texture artwork;
    public Entity owner;
    public DiceSlotData[] slots;
    public DiceData[] startingDice;
    public GameObject slotPrefab;
    private Vector3 startSlotPosition = new Vector3(-3, 0, -3);

    public void CreateSlots(Transform tabletMain)
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
            Transform targetSlot = tabletMain.Find(slotName);

            GameObject diceSlotInstance = Instantiate(slotPrefab, targetSlot);
            diceSlotInstance.transform.localPosition = Vector3.zero;

            diceSlotInstance.name = $"SlotInstance{i + 1}";

            DiceSlotController controller = diceSlotInstance.GetComponent<DiceSlotController>();
            controller.owner = owner;
            controller.SetData(slots[i]);
        }
    }
}
