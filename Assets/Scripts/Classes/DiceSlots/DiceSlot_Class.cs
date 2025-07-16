namespace Diceonomicon

using UnityEngine;

public abstract class DiceSlot: MonoBehaviour
{
    public bool Filled;
    public bool Protection;
    public int Mult;
    public int Priority;
    public string Tag;

    public void DetectLinks() {
        Debug.Log("DiceSlot.DetectLinks");
    }
}
