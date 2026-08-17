using UnityEngine;

[CreateAssetMenu(fileName = "ChainDie_Data", menuName = "Dice/ChainDie")]
public class ChainDie_Data : DiceData
{
    private int linkCount;

    public override void DoEffect(Die die)
    {
        DetectLinksDown(die.transform.position, die);
        DetectLinksUp(die.transform.position, die);
        if (linkCount > 0)
        {
            die.parentSlot.tempMult += 1;
        }
    }

    public void DetectLinksDown(Vector3 pos, Die die)
    {
        Vector3 rayPosition = new Vector3(pos.x, pos.y + 0.1f, pos.z - 0.8f);
        Ray raydown = new Ray(rayPosition, Vector3.down);

        if (Physics.Raycast(raydown, out RaycastHit hit, 666))
        {
            DiceSlotController hitSlot = hit.collider.GetComponent<DiceSlotController>();

            if (hitSlot != null)
            {
                if (hitSlot.slottedDie != null && die.parentSlot.slotTag == hitSlot.slotTag)
                {
                    hitSlot.tempMult += 1;
                    linkCount += 1;
                    DetectLinksDown(rayPosition, die);
                }
            }
        }
    }

    public void DetectLinksUp(Vector3 pos, Die die)
    {
        Vector3 rayPosition = new Vector3(pos.x, pos.y + 0.1f, pos.z + 0.8f);
        Ray raydown = new Ray(rayPosition, Vector3.down);

        if (Physics.Raycast(raydown, out RaycastHit hit, 666))
        {
            DiceSlotController hitSlot = hit.collider.GetComponent<DiceSlotController>();

            if (hitSlot != null)
            {
                if (hitSlot.slottedDie != null && die.parentSlot.slotTag == hitSlot.slotTag)
                {
                    hitSlot.tempMult += 1;
                    linkCount += 1;
                    DetectLinksUp(rayPosition, die);
                }
            }
        }
    }
}
