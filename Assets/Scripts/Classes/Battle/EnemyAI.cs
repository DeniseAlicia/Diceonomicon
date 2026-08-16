using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    public BattleSceneManager sceneManager;

    public List<DiceSlotController> emptySlots;
    public List<Die> dice;
    public List<int> directions;

    public Opponent opponent;
    public GameObject diePrefab;
    private int maxDice;

    private int column1Count;
    private int column2Count;
    private int column3Count;

    [Header("AI Parameters")]
    public int columnLimit;
    public int buffWeight;
    public int linkWeight;

    public enum DiceSortOrder { Ascending, Descending, Random }
    public DiceSortOrder diceSortOrder = DiceSortOrder.Descending;

    public void GetEmptySlots()
    {
        foreach (DiceSlotController slot in emptySlots)
        {
            slot.synergy = slot.slotData.synergy;
        }

        emptySlots = new List<DiceSlotController>();

        float[] columnStartPositions = new float[] { -7.65f, -6.9f, -6.1f };
        for (int j = 0; j < columnStartPositions.Length; j++)
        {
            float columnPosX = columnStartPositions[j];
            Vector3 raycastVector = new Vector3(columnPosX, 3f, 3.6f);

            for (int i = 0; i < 9; i++)
            {
                float zJump = i * -0.8f;
                Vector3 rayPosition = new Vector3(columnPosX + 14.3f, raycastVector.y, raycastVector.z + zJump);
                Ray ray = new Ray(rayPosition, Vector3.down);

                //Debug.DrawRay(rayPosition, Vector3.down * 2, Color.purple, 666);
                if (Physics.Raycast(ray, out RaycastHit hit2, 666))
                {
                    DiceSlotController slotController = hit2.collider.GetComponent<DiceSlotController>();
                    if (slotController != null)
                    {
                        if (slotController.tag != "Empty")
                        {
                            slotController.synergy += 1;

                            if (slotController.tag == "Buff")
                            {
                                DetectBuffNeighbors(slotController);
                            }
                        }
                        emptySlots.Add(slotController);
                    }
                }
            }
        }

        foreach (DiceSlotController slot in emptySlots)
        {
            DetectSynergyDown(slot);
            DetectSynergyUp(slot);
        }

        SortSlots();
    }

    private void SortSlots()
    {
        System.Random rng = new System.Random();

        emptySlots = emptySlots
    .OrderByDescending(slot => slot.synergy)
    .ThenBy(slot => rng.Next())
    .ToList();
    }

    public void RollDice()
    {
        dice = new List<Die>();
        Vector3 startPosition = new Vector3(12f, 5f, -5f);
        float distance = 0.5f;

        for (int i = 0; i < opponent.drawnDice.Count; i++)
        {
            DiceData dieData = opponent.drawnDice[i];

            float overflow = Mathf.Floor(i / 3f);
            float spacing = (i - overflow * 3) * distance;

            Vector3 diePos = startPosition;
            diePos.x += spacing;
            diePos.z += overflow * distance;

            GameObject dieObject = Instantiate(diePrefab, diePos, Quaternion.identity);

            Die die = dieObject.GetComponent<Die>();
            die.SetData(dieData);

            dice.Add(die);
        }

        foreach (Die dieInstance in dice)
        {
            dieInstance.Roll(-0.2f);
        }

        maxDice = dice.Count();

        if (columnLimit == 0)
        {
            columnLimit = Mathf.CeilToInt(maxDice / 3) + 1;
        }

        GetEmptySlots();
        StartCoroutine(PlaceDieDelay());
    }

    private IEnumerator PlaceDieDelay()
    {
        float delay = 1f;
        yield return new WaitForSeconds(delay);

        foreach (Die die in dice)
        {
            die.vfx.gameObject.SetActive(true);
            die.vfx.Play();
            die.GetSideFacingUp();
            die.isResting = true;
            die.isDraggable = false;
            die.rigidBody.isKinematic = true;
            die.rigidBody.useGravity = false;
        }

        switch (diceSortOrder)
        {
            case DiceSortOrder.Ascending:
                dice = dice.OrderBy(d => d.value).ToList();
                break;
            case DiceSortOrder.Descending:
                dice = dice.OrderByDescending(d => d.value).ToList();
                break;
            case DiceSortOrder.Random:
                dice = dice.OrderBy(d => UnityEngine.Random.value).ToList();
                break;
        }

        PlaceBuffDice();
        SortSlots();

        column1Count = 0;
        column2Count = 0;
        column3Count = 0;

        PlaceDice();
        PlaceNeutralDice();

        if (dice.Count() > 0)
        {
            PlaceLeftoverDice();
        }
    }

    public void PlaceBuffDice()
    {
        foreach (DiceSlotController slot in emptySlots)
        {
            if (slot.tag == "Buff")
            {
                foreach (Die die in dice)
                {
                    if (die.dieTags.Contains(slot.tag))
                    {
                        die.transform.SetParent(slot.transform);
                        die.transform.localPosition = new Vector3(0, 3, 0);
                        die.transform.localScale = die.scale;

                        slot.isFilled = true;
                        slot.slottedDie = die;
                        die.isPlaced = true;
                        die.parentSlot = slot;
                        die.MoveToLayer("BattleTablets");

                        FindBuffTargetSlots(die, slot);

                        dice.Remove(die);
                        break;
                    }
                }
            }
        }
    }

    public void PlaceDice()
    {

        for (int i = 0; i < emptySlots.Count; i++)
        {
            DiceSlotController slot = emptySlots[i];
            int columnIndex;

            if (slot.isFilled)
            {
                continue;
            }

            if (slot.transform.position.x < 7.3f) columnIndex = 1;
            else if (slot.transform.position.x < 8.1f) columnIndex = 2;
            else columnIndex = 3;

            if ((columnIndex == 1 && column1Count >= columnLimit) ||
                (columnIndex == 2 && column2Count >= columnLimit) ||
                (columnIndex == 3 && column3Count >= columnLimit))
            {
                continue;
            }

            foreach (Die die in dice)
            {
                if (die.dieTags.Contains(slot.tag))
                {
                    die.transform.SetParent(slot.transform);
                    die.transform.localPosition = new Vector3(0, 3, 0);
                    die.transform.localScale = die.scale;

                    slot.isFilled = true;
                    slot.slottedDie = die;
                    die.isPlaced = true;
                    die.parentSlot = slot;
                    die.MoveToLayer("BattleTablets");

                    if (columnIndex == 1) column1Count++;
                    else if (columnIndex == 2) column2Count++;
                    else column3Count++;

                    CheckDieUpDown(die);

                    dice.Remove(die);

                    // Restart recursion after placing a die
                    PlaceDice();
                    return;
                }
            }
        }
    }

    public void PlaceNeutralDice()
    {

        for (int i = 0; i < emptySlots.Count; i++)
        {
            DiceSlotController slot = emptySlots[i];
            int columnIndex;

            if (slot.isFilled)
            {
                continue;
            }

            if (slot.transform.position.x < 7.3f) columnIndex = 1;
            else if (slot.transform.position.x < 8.1f) columnIndex = 2;
            else columnIndex = 3;

            if ((columnIndex == 1 && column1Count >= columnLimit) ||
                (columnIndex == 2 && column2Count >= columnLimit) ||
                (columnIndex == 3 && column3Count >= columnLimit))
            {
                continue;
            }

            foreach (Die die in dice)
            {
                if (die.dieTags.Contains("Neutral") && slot.tag == "Spell" || die.dieTags.Contains("Neutral") && slot.tag == "Block" || die.dieTags.Contains("Neutral") && slot.tag == "Damage")
                {
                    die.transform.SetParent(slot.transform);
                    die.transform.localPosition = new Vector3(0, 3, 0);
                    die.transform.localScale = die.scale;

                    slot.isFilled = true;
                    slot.slottedDie = die;
                    die.isPlaced = true;
                    die.parentSlot = slot;
                    die.MoveToLayer("BattleTablets");

                    if (columnIndex == 1) column1Count++;
                    else if (columnIndex == 2) column2Count++;
                    else column3Count++;

                    CheckDieUpDown(die);

                    dice.Remove(die);

                    // Restart recursion after placing a die
                    PlaceNeutralDice();
                    return;
                }
            }
        }
    }

    public void PlaceLeftoverDice()
    {
        foreach (DiceSlotController slot in emptySlots)
        {
            if (slot.isFilled)
            {
                continue;
            }

            foreach (Die die in dice)
            {
                if (die.dieTags.Contains(slot.tag) || die.dieTags.Contains("Neutral") && slot.tag == "Spell" || die.dieTags.Contains("Neutral") && slot.tag == "Block" || die.dieTags.Contains("Neutral") && slot.tag == "Damage")
                {
                    die.transform.SetParent(slot.transform);
                    die.transform.localPosition = new Vector3(0, 3, 0);
                    die.transform.localScale = die.scale;

                    slot.isFilled = true;
                    slot.slottedDie = die;
                    die.isPlaced = true;
                    die.parentSlot = slot;
                    die.MoveToLayer("BattleTablets");

                    CheckDieUpDown(die);

                    dice.Remove(die);
                    break;
                }
            }
        }
    }

    public void FindBuffTargetSlots(Die die, DiceSlotController slot)
    {
        DieAction.ValueToAngle(die, directions);
        List<int> dirAngles = directions;

        List<Die> targets = new List<Die>();
        int maxDistance = 1;

        foreach (int angle in dirAngles)
        {
            Vector3 direction = Quaternion.Euler(-35, angle, -35) * Vector3.back;

            Transform originTransform = slot.transform;

            Vector3 raycastVector = new Vector3(originTransform.position.x, originTransform.position.y + 0.5f, originTransform.position.z);

            Ray ray = new Ray(raycastVector, originTransform.TransformDirection(direction));
            //Debug.DrawRay(raycastVector, originTransform.TransformDirection(direction) * 3, Color.red, 666f);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                DiceSlotController hitSlot = hit.collider.GetComponent<DiceSlotController>();
                if (hitSlot != null)
                {
                    if (hitSlot.tag != "Buff")
                    {
                        hitSlot.synergy += buffWeight;
                    }

                }
            }
        }
    }

    public void DetectSynergyDown(DiceSlotController slot)
    {
        Vector3 rayPosition = new Vector3(slot.transform.position.x, slot.transform.position.y + 0.2f, slot.transform.position.z - 0.8f);
        Ray raydown = new Ray(rayPosition, Vector3.down);

        if (Physics.Raycast(raydown, out RaycastHit hit, 66))
        {
            DiceSlotController hitSlot = hit.collider.GetComponent<DiceSlotController>();
            if (hitSlot != null && hitSlot.tag == slot.tag)
            {
                hitSlot.synergy += linkWeight;
                DetectSynergyDown(hitSlot);
            }
        }
    }

    public void DetectSynergyUp(DiceSlotController slot)
    {
        Vector3 rayPosition = new Vector3(slot.transform.position.x, slot.transform.position.y + 0.2f, slot.transform.position.z + 0.8f);
        Ray rayup = new Ray(rayPosition, Vector3.down);

        if (Physics.Raycast(rayup, out RaycastHit hit, 66))
        {
            DiceSlotController hitSlot = hit.collider.GetComponent<DiceSlotController>();
            if (hitSlot != null && hitSlot.tag == slot.tag)
            {
                hitSlot.synergy += linkWeight;
                DetectSynergyUp(hitSlot);
            }
        }
    }

    public void CheckDieUpDown(Die die)
    {
        Vector3 rayPosition = new Vector3(die.transform.position.x, die.transform.position.y + 0.2f, die.transform.position.z + 0.8f);
        Ray rayup = new Ray(rayPosition, Vector3.forward);

        if (Physics.Raycast(rayup, out RaycastHit hit, 66))
        {
            DiceSlotController hitSlot = hit.collider.GetComponent<DiceSlotController>();
            if (hitSlot != null && die.dieTags.Contains(hitSlot.tag))
            {
                hitSlot.synergy += 1;
            }
        }

        Vector3 rayPosition2 = new Vector3(die.transform.position.x, die.transform.position.y + 0.2f, die.transform.position.z - 0.8f);
        Ray raydown = new Ray(rayPosition2, Vector3.forward);

        if (Physics.Raycast(raydown, out RaycastHit hit2, 66))
        {
            DiceSlotController hitSlot = hit2.collider.GetComponent<DiceSlotController>();
            if (hitSlot != null && die.dieTags.Contains(hitSlot.tag))
            {
                hitSlot.synergy += 1;
            }
        }

        SortSlots();
    }

    public void DetectBuffNeighbors(DiceSlotController slot)
    {
        for (int j = 0; j < 3; j++)
        {
            float xJump = j * 0.8f;
            Vector3 raycastVector = new Vector3(slot.transform.position.x - 0.8f + xJump, slot.transform.position.y + 0.2f, slot.transform.position.z);

            for (int i = 0; i < 3; i++)
            {
                float zJump = i * -0.8f;
                Vector3 rayPosition = new Vector3(raycastVector.x, raycastVector.y, raycastVector.z + zJump);
                Ray ray = new Ray(rayPosition, Vector3.down);

                if (Physics.Raycast(ray, out RaycastHit hit2, 666))
                {
                    DiceSlotController slotController = hit2.collider.GetComponent<DiceSlotController>();
                    if (slotController != null)
                    {
                        if (slotController.tag != "Empty")
                        {
                            slot.synergy += 1;
                        }
                    }
                }
            }
        }
    }
}
