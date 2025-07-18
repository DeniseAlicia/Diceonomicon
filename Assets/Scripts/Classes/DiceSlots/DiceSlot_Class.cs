namespace Diceonomicon
{

    using UnityEngine;

    public abstract class DiceSlot : MonoBehaviour
    {
        public bool filled;
        public bool protection;
        public int mult;
        public string type;
        public string owner;
        public Die slottedDie;


        public void DetectLinks()
        {
            Debug.Log("DiceSlot.DetectLinks");
        }

        public abstract void DoEffect();
    }
}