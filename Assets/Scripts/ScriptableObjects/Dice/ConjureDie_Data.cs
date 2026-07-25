using Unity.VisualScripting;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ConjureDie_Data", menuName = "Dice/ConjureDie")]
public class ConjureDie_Data : DiceData
{

    public override void DoEffect(Die currentDie)
    {
        DiceSlotController currentSlot = currentDie.parentSlot;
        DiceSlotController nextSlot = FindNextSlot(currentSlot);

        if (nextSlot == null)
            return;

        if (currentDie.dieTags.Contains(nextSlot.tag) || currentDie.dieTags.Contains("Colorless"))
        {

            GameObject dieInstance = Instantiate(prefab, nextSlot.transform.position, Quaternion.identity);
            Die die = dieInstance.GetComponent<Die>();

            die.nameText = "Conjured Die";
            die.descText = "Value 1";
            //textureRenderer.material.SetTexture("_BaseMap", dieData.texture);
            die.usedTexture = Resources.Load<Texture2D>("Dice/Materials/Used_DiceBlank_Texture");
            die.range = new int[] { 1, 1, 1, 1, 1, 1 };
            die.dieTags = new string[] { "Damage", "Spell", "Block" };

            DieAction.RangeToValue(die);

            DiceSlotController chosenSlot = nextSlot;

            if (chosenSlot.tag == "Spell")
            {
                Texture texture = Resources.Load<Texture2D>("Dice/Materials/Dummy_DiceSpell_Texture");
                die.textureRenderer.material.SetTexture("_BaseMap", texture);
            }
            else if (chosenSlot.tag == "Damage")
            {
                Texture texture = Resources.Load<Texture2D>("Dice/Materials/Dummy_DiceDamage_Texture");
                die.textureRenderer.material.SetTexture("_BaseMap", texture);
            }
            else if (chosenSlot.tag == "Block")
            {
                Texture texture = Resources.Load<Texture2D>("Dice/Materials/Dummy_DiceBlock_Texture");
                die.textureRenderer.material.SetTexture("_BaseMap", texture);
            }

            die.GetSideFacingUp();
            die.isResting = true;
            die.isDraggable = false;
            die.rigidBody.isKinematic = true;
            die.rigidBody.useGravity = false;

            die.transform.SetParent(chosenSlot.transform);
            die.transform.localPosition = new Vector3(0, 3, 0);
            die.transform.localScale = new Vector3(6f, 6f, 6f);

            chosenSlot.isFilled = true;
            chosenSlot.slottedDie = die;
            die.isPlaced = true;
            die.MoveToLayer("BattleTablets");

            die.isPlaced = true;
        }
    }

    private DiceSlotController FindNextSlot(DiceSlotController currentSlot)
    {
        Vector3 origin = currentSlot.transform.position; // cast from above for sure
        Vector3 rayStart = new Vector3(origin.x + 1f, origin.y + 1f, origin.z);

        Ray ray = new Ray(rayStart, Vector3.down * 5);
        Debug.DrawRay(rayStart, Vector3.down * 5, Color.magenta, 555f);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f)) // cast down max 10 units
        {
            DiceSlotController slotController = hit.collider.GetComponent<DiceSlotController>();
            if (slotController != null && !slotController.isFilled)
            {
                // Found the slot 3.33 units away
                return slotController;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
}
