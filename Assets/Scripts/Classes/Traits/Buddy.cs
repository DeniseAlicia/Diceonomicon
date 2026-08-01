using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Buddy : Trait
{
    private BattleSceneManager battleSceneManager;
    private TabletController tablet;
    private Die buddyDie;

    public void Start()
    {
        battleSceneManager = FindFirstObjectByType<BattleSceneManager>();
        tablet = GetComponent<TabletController>();

        description = "Fill a random slot with a dummy die";
        tablet.descText.text = description;

        // sceneStart = true;
        roundStart = true;
        //acvitveCombatStart = true;
        // placementDone = true;
        // acvitveCombatEnd = true;

        if (sceneStart)
        {
            BattleSceneManager.OnSceneStart.AddListener(OnSceneStart);
        }
        if (roundStart)
        {
            BattleSceneManager.OnRoundStart.AddListener(OnRoundStart);
        }
        if (placementDone)
        {
            BattleSceneManager.OnPlacementDone.AddListener(OnPlacementDone);
        }
        if (acvitveCombatStart)
        {
            BattleSceneManager.OnAcvitveCombatStart.AddListener(OnAcvitveCombatStart);
        }
        if (acvitveCombatEnd)
        {
            BattleSceneManager.OnAcvitveCombatEnd.AddListener(OnAcvitveCombatEnd);
        }
    }


    public override void OnSceneStart()
    {
        Debug.Log("Triggered on SceneStart");
    }

    public override void OnRoundStart()
    {
        List<DiceSlotController> randomSlots = new List<DiceSlotController>();

        foreach (DiceSlotController slot in tablet.tabletSlots)
        {
            if (!slot.isFilled && slot.tag != "Buff" && slot.tag != "Empty")
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
        die.range = new int[] { 1, 1, 1, 1, 1, 1 };
        die.dieTags = new string[] { "Damage", "Spell", "Block" };

        DieAction.RangeToValue(die);

        DiceSlotController chosenSlot = randomSlots[0];

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

        StartCoroutine(SetStats(die));
    }

    public override void OnPlacementDone()
    {
        Debug.Log("Triggered on PlacementDone");
    }

    public override void OnAcvitveCombatStart()
    {

    }

    public override void OnAcvitveCombatEnd()
    {
        Debug.Log("Triggered on AcvitveCombatEnd");
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
}
