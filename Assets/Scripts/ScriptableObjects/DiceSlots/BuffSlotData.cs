using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "BuffSlotData", menuName = "DiceSlots/BuffSlotData")]
public class BuffSlotData : DiceSlotData
{
    public List<int> directions;

    public void TranslateDieValue(Die die)
    {
        directions.Clear();

        if (die.value > 0 && die.value < 7)
        {
            die.value = die.range[die.value - 1];
        }

        string valueString = die.value.ToString();
        foreach (char x in valueString)
        {
            int newValue = Int32.Parse(x.ToString());
            int angle = newValue * 45 - 45 - die.dieRotation;
            directions.Add(angle);
            Debug.Log(angle);
        }
    }

    public List<Die> FindTargetDie(Die die, DiceSlotController slot)
    {
        TranslateDieValue(die);
        List<int> dirAngles = directions;
        List<Die> targets = new List<Die>();
        int maxDistance = 1;

        foreach (int angle in dirAngles)
        {
            Vector3 direction = Quaternion.Euler(20, angle, -20) * Vector3.back;
            Transform originTransform = slot.transform;

            Ray ray = new Ray(originTransform.position, originTransform.TransformDirection(direction));
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                //Debug.Log($"Ray hit: {hit.collider.name} at {hit.point}");

                Die dieComponent = hit.collider.GetComponent<Die>();
                if (dieComponent != null)
                {
                    if (dieComponent.dieTag != "Buff")
                    {
                        targets.Add(dieComponent);
                    }
                }
                else
                {
                    return null;
                }
            }
            // return targets;
        }
        return targets;
    }
}
