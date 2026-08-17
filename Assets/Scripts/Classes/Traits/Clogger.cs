using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Clogger : Trait
{
    private Texture2D rockTexture;
    private List<Die> cloggerDice = new List<Die>();

    public void Start()
    {
        tablet = GetComponent<TabletController>();
        rockTexture = Resources.Load<Texture2D>("Dice/Materials/Rock_Texture");

        description = "Fills three random slots with rocks";
        tablet.descText.text = description;

        BattleSceneManager.OnRoundStart.AddListener(OnRoundStart);
        OnRoundStart();
    }

    public void OnRoundStart()
    {
        cloggerDice = new List<Die>();
        List<DiceSlotController> randomSlots = new List<DiceSlotController>();
        List<DiceSlotController> allSlots = new List<DiceSlotController>(FindObjectsByType<DiceSlotController>(FindObjectsSortMode.None));

        foreach (DiceSlotController slot in allSlots)
        {
            if (slot.owner != tablet.owner && !slot.isFilled && slot.slotTag != "Empty")
            {
                randomSlots.Add(slot);
            }
        }

        Shuffle(randomSlots);

        for (int i = 0; i < 3; i++)
        {
            GameObject dieInstance = Instantiate(Player.Instance.diePrefab, randomSlots[i].transform.position, Quaternion.identity);
            Die die = dieInstance.GetComponent<Die>();

            die.nameText = "Rock";
            die.descText = "This slot is blocked.";
            die.usedTexture = rockTexture;

            die.range = new int[] { 0, 0, 0, 0, 0, 0 };
            die.dieTags = new string[] { "Neutral" };

            DieAction.RangeToValue(die);

            DiceSlotController chosenSlot = randomSlots[i];

            die.textureRenderer.material.SetTexture("_BaseMap", rockTexture);
            die.GetSideFacingUp();
            die.isResting = true;
            die.isDraggable = false;
            die.rigidBody.isKinematic = true;
            die.rigidBody.useGravity = false;

            die.transform.SetParent(chosenSlot.transform);
            die.transform.localPosition = new Vector3(0, 3, 0);
            die.transform.localScale = die.scale * 1.3f;

            chosenSlot.isFilled = true;
            chosenSlot.slottedDie = die;
            die.isPlaced = true;
            die.MoveToLayer("BattleTablets");

            die.statuses.Add(Status.Inactive);

            StartCoroutine(SetStats(die));
            cloggerDice.Add(die);
        }


        foreach (Die die in cloggerDice)
        {
            die.range = new int[] { 0, 0, 0, 0, 0, 0 };
            die.dieTags = new string[] { "" };
            DieAction.RangeToValue(die);

            foreach (Transform childSide in die.GetDiceSides())
            {
                GameObject child = childSide.gameObject;

                if (!int.TryParse(child.name, out int index))
                {
                    continue;
                }

                GameObject side = child.transform.GetChild(0).gameObject;
                side.SetActive(false);
            }
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
