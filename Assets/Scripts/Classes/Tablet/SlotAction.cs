using UnityEngine;

public static class SlotAction
{
    public static void DetectLinksUp(DiceSlotController slot, int linkWeight)
    {
        Vector3 rayPosition = new Vector3(slot.transform.position.x, slot.transform.position.y + 0.2f, slot.transform.position.z + 0.8f);
        Ray rayup = new Ray(rayPosition, Vector3.down);

        if (Physics.Raycast(rayup, out RaycastHit hit))
        {
            DiceSlotController hitSlot = hit.collider.GetComponent<DiceSlotController>();
            if (hitSlot != null && hitSlot.tag == slot.tag)
            {
                hitSlot.synergy += linkWeight;
                DetectLinksUp(hitSlot, linkWeight);
            }
        }
    }
}
