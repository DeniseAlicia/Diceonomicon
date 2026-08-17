using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Buddy : Trait
{
    private Die buddyDie;

    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "Fill a random slot with a dummy die";
        tablet.descText.text = description;

        BattleSceneManager.OnRoundStart.AddListener(OnRoundStart);
    }

    public void OnRoundStart()
    {
        List<DiceSlotController> randomSlots = new List<DiceSlotController>();

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (!slot.isFilled && slot.slotTag != "Buff" && slot.slotTag != "Empty")
            {
                randomSlots.Add(slot);
            }
        }

        Shuffle(randomSlots);

        GameObject dieInstance = Instantiate(Player.Instance.diePrefab, randomSlots[0].transform.position, Quaternion.identity);
        Die die = dieInstance.GetComponent<Die>();

        die.nameText = "Buddy Die";
        die.descText = "Value 1";
        //textureRenderer.material.SetTexture("_BaseMap", dieData.texture);
        die.usedTexture = Resources.Load<Texture2D>("Dice/Materials/Used_DiceBlank_Texture");

        DiceSlotController chosenSlot = randomSlots[0];

        if (chosenSlot.slotTag == "Spell")
        {
            die.data = Resources.Load<DiceData>("Dice/BasicSpellDie");
            die.dieTags = new string[] { "Spell" };
            Texture texture = Resources.Load<Texture2D>("Dice/Materials/Dummy_DiceSpell_Texture");
            die.textureRenderer.material.SetTexture("_BaseMap", texture);
        }
        else if (chosenSlot.slotTag == "Damage")
        {
            die.data = Resources.Load<DiceData>("Dice/BasicDamageDie");
            die.dieTags = new string[] { "Damage" };
            Texture texture = Resources.Load<Texture2D>("Dice/Materials/Dummy_DiceDamage_Texture");
            die.textureRenderer.material.SetTexture("_BaseMap", texture);
        }
        else if (chosenSlot.slotTag == "Block")
        {
            die.data = Resources.Load<DiceData>("Dice/BasicBlockDie");
            die.dieTags = new string[] { "Block" };
            Texture texture = Resources.Load<Texture2D>("Dice/Materials/Dummy_DiceBlock_Texture");
            die.textureRenderer.material.SetTexture("_BaseMap", texture);
        }

        die.range = new int[] { 1, 1, 1, 1, 1, 1 };
        DieAction.RangeToValue(die);

        die.GetSideFacingUp();
        die.isResting = true;
        die.isDraggable = false;
        die.rigidBody.isKinematic = true;
        die.rigidBody.useGravity = false;

        die.transform.SetParent(chosenSlot.transform);
        die.transform.localPosition = new Vector3(0, 3, 0);
        die.transform.localScale = die.scale;

        chosenSlot.isFilled = true;
        chosenSlot.slottedDie = die;
        die.parentSlot = chosenSlot;
        die.isPlaced = true;
        die.MoveToLayer("BattleTablets");

        StartCoroutine(SetStats(die));
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

    private IEnumerator SetStats(Die die)
    {
        yield return new WaitForSeconds(1);
        die.isPlaced = true;
    }

    public override void UnsubscribeFromEvents()
    {
        BattleSceneManager.OnRoundStart.RemoveListener(OnRoundStart);
    }
}
