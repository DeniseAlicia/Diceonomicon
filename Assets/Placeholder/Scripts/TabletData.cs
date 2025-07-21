using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TabletData", menuName = "Data/TabletData")]
public class TabletData : ScriptableObject
{
    public bool enemy = false;

    public new string name;
    public string desc;
    public Texture artwork;

    public TabletSlotData[] slots;
    public GameObject slotPrefab;
    private Vector3 startSlotPosition = new Vector3(-3, 0, -3);

    public void CreateSlots(Transform tabletMain)
    {
        Vector3 currentSlotPosition = startSlotPosition;

        for (int i = 0; i < slots.Length; i++)
        {
            string slotName = $"Slot{i + 1}";
            Transform targetSlot = tabletMain.Find(slotName);

            GameObject tabletSlotInstance = Instantiate(slotPrefab, targetSlot);
            tabletSlotInstance.transform.localPosition = Vector3.zero;

            tabletSlotInstance.name = $"SlotInstance{i + 1}";

            TabletSlotController controller = tabletSlotInstance.GetComponent<TabletSlotController>();
            controller.SetData(slots[i]);
        }
    }
}
