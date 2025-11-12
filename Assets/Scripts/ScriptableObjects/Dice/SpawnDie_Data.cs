using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnDie_Data", menuName = "Dice/SpawnDie")]
public class SpawnDie_Data : DiceData
{

    public override void DoEffect(Die currentDie)
    {
        List<DiceSlotController> allSlots = BattleSceneManager.Instance.opponent.ai.emptySlots;
        List<DiceSlotController> emptySlots = new();

        foreach (DiceSlotController slot in allSlots)
        {
            if (!slot.isFilled && currentDie.dieTags.Contains(slot.tag))
            {
                emptySlots.Add(slot);
            }
        }

        Shuffle(emptySlots);

        if (emptySlots.Count > 0)
        {
            DiceSlotController chosenSlot = emptySlots[0];

            GameObject dieInstance = Instantiate(prefab, emptySlots[0].transform.position, Quaternion.identity);
            Die die = dieInstance.GetComponent<Die>();

            die.nameText = "Conjured Die";
            die.descText = "Value 1";
            //textureRenderer.material.SetTexture("_BaseMap", dieData.texture);
            die.usedTexture = Resources.Load<Texture2D>("Dice/Materials/Used_DiceBlank_Texture");
            die.range = new int[] { 1, 1, 1, 1, 1, 1 };
            die.dieTags = new string[] { "Damage", "Spell", "Block" };

            die.TranslateValueAtStart();



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
            Opponent opponent = GameObject.Find("Opponent").GetComponent<Opponent>();
            die.parentSlot = chosenSlot;
            die.parentSlot.owner = opponent;
            die.MoveToLayer("BattleTablets");

            die.isPlaced = true;
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}
