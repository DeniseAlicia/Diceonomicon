using UnityEngine;

[CreateAssetMenu(fileName = "ExplodingDie_Data", menuName = "Dice/ExplodingDie")]
public class ExplodingDie_Data : DiceData
{
    public override void DoEffect(Die die)
    {
        DiceSlotController slot = die.parentSlot;
        int damage = 0;

        for (int j = 0; j < 3; j++)
        {
            float xJump = j * 1f;
            Vector3 raycastVector = new Vector3(slot.transform.position.x - 1f + xJump, slot.transform.position.y + 1f, slot.transform.position.z);

            for (int i = 0; i < 3; i++)
            {
                float zJump = i * -1f;
                Vector3 rayPosition = new Vector3(raycastVector.x, raycastVector.y, raycastVector.z + 1f + zJump);
                Ray ray = new Ray(rayPosition, Vector3.down * 3);
                Debug.DrawRay(rayPosition, Vector3.down  * 3, Color.blue, 555f);
                if (Physics.Raycast(ray, out RaycastHit hit, 666))
                {
                    Die hitDie = hit.collider.GetComponent<Die>();
                    if (hitDie != null && hitDie != die)
                    {
                        damage += 1;
                    }

                }
            }
        }

        die.didDamage = damage;
    }

}
