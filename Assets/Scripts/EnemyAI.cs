using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    public List<DiceSlotController> emptySlots;
    public List<Die> dice;
    public Opponent opponent;
    public GameObject columnMaster;
    public GameObject diePrefab;

    public void GetEnemySlots()
    {
        emptySlots = new List<DiceSlotController>();

        float[] columnStartPositions = new float[] { -7.65f, -6.9f, -6.1f };

        for (int j = 0; j < columnStartPositions.Length; j++)
        {
            float columnPosX = columnStartPositions[j];

            for (int i = 0; i < 9; i++)
            {
                float yJump = i * -0.8f;

                Vector3 rayPosition = new Vector3(columnPosX + 14.3f, columnMaster.transform.position.y + yJump, columnMaster.transform.position.z);
                Ray ray = new Ray(rayPosition, Vector3.forward);

                if (Physics.Raycast(ray, out RaycastHit hit2, 666))
                {
                    DiceSlotController slotController = hit2.collider.GetComponent<DiceSlotController>();
                    if (slotController != null)
                    {
                        emptySlots.Add(slotController);
                    }
                }
            }
            // Debug.Log("Enemy DiceSlots: " + string.Join(", ", emptySlots));
        }
    }

    public void PlaceDie(Die die)
    {
        List<DiceSlotController> color = (
             from slot in emptySlots
             where die.dieTags.Contains(slot.tag) && slot.isFilled == false
             select slot
        ).ToList();

        int rdm = UnityEngine.Random.Range(0, color.Count);

        DiceSlotController filledSlot = color[rdm];

        die.transform.SetParent(filledSlot.transform);
        die.transform.localPosition = new Vector3(0, 3, 0);
        die.transform.Rotate(new Vector3(-90, 0, 0), Space.World);
        die.transform.Rotate(new Vector3(0, 0, die.dieRotation), Space.World);
        die.transform.localScale = new Vector3(6f, 6f, 6f);

        filledSlot.isFilled = true;
        filledSlot.slottedDie = die;
        die.isPlaced = true;

        color = new List<DiceSlotController>();
    }

    public void RollDice()
    {
        GetEnemySlots();

        List<Die> dice = new List<Die>();
        Vector3 startPosition = new Vector3(2f, 5f, -5f);
        float distance = 0.5f;

        for (int i = 0; i < opponent.drawnDice.Count; i++)
        {
            DiceData dieData = opponent.drawnDice[i];

            float overflow = Mathf.Floor(i / 3f);
            float spacing = (i - overflow * 3) * distance;

            Vector3 diePos = startPosition;
            diePos.x += spacing;
            diePos.z += overflow * distance;

            // Instantiate prefab at the calculated position
            GameObject dieObject = Instantiate(diePrefab, diePos, Quaternion.identity);

            // Set data on the die script
            Die die = dieObject.GetComponent<Die>();
            die.SetData(dieData);

            // Add dice to die Class list
            dice.Add(die);
        }

        foreach (Die dieInstance in dice)
        {
            dieInstance.Roll(-0.2f);
            StartCoroutine(PlaceDieDelay(dieInstance));
        }
        ;
    }

    private IEnumerator PlaceDieDelay(Die die)
    {
        float delay = 3f;
        yield return new WaitForSeconds(delay);
        die.GetSideFacingUp();
        die.isResting = true;
        die.isDraggable = false;
        die.rigidBody.isKinematic = true;
        die.rigidBody.useGravity = false;
        PlaceDie(die);
        die.MoveToLayer("BattleTablets");
    }
}

