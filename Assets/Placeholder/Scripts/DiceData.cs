using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DiceData", menuName = "Data/DiceData")]
public class DiceData : ScriptableObject
{
    public new string name;
    public string desc;

    public Image artwork;

    public string[] slots;
    public GameObject prefab;

    public bool enemy = false;

    public void CreateSlots()
    {
        foreach (string slot in slots)
        {
            Instantiate(prefab);
        }
    }
}
