using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BuffSlotData", menuName = "DiceSlots/BuffSlotData")]
public class BuffSlotData : DiceSlotData
{
    public List<int> directions;

    public void TranslateDieValue(Die die)
    {
        // string x = "0";
        directions = new List<int> { 0, 90 };
    }

    private Die FindTargetDie(Die die, DiceSlotController slot)
    {
        TranslateDieValue(die);
        List<int> dirAngles = directions;
        int maxDistance = 1;

        foreach (int angle in dirAngles)
        {
            Vector3 direction = Quaternion.Euler(20, die.dieRotation + angle, -20) * Vector3.back;
            Transform originTransform = slot.transform;

            Ray ray = new Ray(originTransform.position, originTransform.TransformDirection(direction));
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                Debug.Log($"Ray hit: {hit.collider.name} at {hit.point}");

                Die dieComponent = hit.collider.GetComponent<Die>();
                if (dieComponent != null)
                {
                    return dieComponent;
                }
                else
                {
                    Debug.Log("Hit object does not have a Die component.");
                    return null;
                }
            }
            return null;
        }
        return null;
    }
}
