using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Horned : Trait
{
    public void Start()
    {
        tablet = GetComponent<TabletController>();

        description = "Fill two random slots with a neutral die";
        tablet.descText.text = description;

        OnRoundStart();
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

        for (int i = 0; i < 2 && i < randomSlots.Count; i++)
        {
            GameObject dieInstance = Instantiate(Player.Instance.diePrefab, randomSlots[i].transform.position, Quaternion.identity);
            Die die = dieInstance.GetComponent<Die>();

            die.data = Resources.Load<DiceData>("Dice/BasicNeutralDie");

            die.nameText = "Horn";
            die.descText = "Value 3";
            //textureRenderer.material.SetTexture("_BaseMap", dieData.texture);
            die.usedTexture = Resources.Load<Texture2D>("Dice/Materials/Used_DiceBlank_Texture");

            Texture texture = Resources.Load<Texture2D>("Dice/Materials/DiceBlank_Texture");
            die.textureRenderer.material.SetTexture("_BaseMap", texture);
            die.range = new int[] { 3, 3, 3, 3, 3, 3 };
            die.dieTags = new string[] { "Neutral" };

            DieAction.RangeToValue(die);

            DiceSlotController chosenSlot = randomSlots[i];

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
